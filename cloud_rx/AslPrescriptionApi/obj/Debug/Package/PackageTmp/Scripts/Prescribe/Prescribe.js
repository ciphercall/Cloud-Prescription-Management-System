var PrescribeApp = angular.module("PrescribeApp", ['ui.bootstrap']);

PrescribeApp.controller("ApiPrescribeController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;






    //// Inital Grid view load 
    //var compid = $('#txtcompid').val();
    //var transno = $('#txt_TransNo').val();
    //$http.get('/api/ApiPrescribeController/Get1stPart/', {
    //    params: {
    //        Compid: compid,
    //        TrnsNo: transno
    //    }
    //}).success(function (data) {
    //    if (data[0].count == 1) {
    //        $scope.FirstPartData = null;
    //    } else {
    //        $scope.FirstPartData = data;
    //    }
    //    $scope.loading = false;
    //});





    $scope.toggleEdit_1ST = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };



    $scope.toggleEdit_Cancel__1ST = function () {
        this.testitem.editMode = !this.testitem.editMode;
        //Grid view load 
        var compid1 = $('#txtcompid').val();
        var transno1 = $('#txt_TransNo').val();
        $http.get('/api/ApiPrescribeController/Get1stPart/', {
            params: {
                Compid: compid1,
                TrnsNo: transno1,
            }
        }).success(function (data) {
            $scope.FirstPartData = data;
            $scope.loading = false;
        });
    };




    // Add Master Data
    $scope.submit = function (event) {
        $scope.loading = false;
        event.preventDefault();
        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            $('#Hiddentxt_RxPID').val("");
            $('#txt_RxpPIDM').val("");
            $('#txt_PatientName').val("");
            alert("Permission Denied!!");
            window.location.reload(true);
        } else {
            var COMPID = $('#txtcompid').val();
            var INSUSERID = $('#txtInsertUserid').val();
            var INSLTUDE = $('#latlon').val();

            var RXPID = $('#Hiddentxt_RxPID').val();
            var TRANSDT = $('#txt_TransDate').val();
            var TRANSNO = $('#txt_TransNo').val();
            var RXPTP = $('#txt_RxpType').val();
            var RXPIDM = $('#txt_RxpPIDM').val();
            var NXTDT = $('#txt_NextDate').val();
            var RXPNM = $('#txt_PatientName').val();
            var REMARKS = $('#txt_Remarks').val();
            var AMOUNT = $('#txt_Amount').val();

            if (TRANSNO != "" && RXPNM != "" && RXPIDM != "") {
                $http.get('/api/ApiPrescribeController/addMasterData/', {
                    params: {
                        Compid: COMPID,
                        insertUserID: INSUSERID,
                        insltude: INSLTUDE,
                        rxpid: RXPID,
                        transDate: TRANSDT,
                        transno: TRANSNO,
                        rxpTp: RXPTP,
                        rxpIDM: RXPIDM,
                        NextDate: NXTDT,
                        rxpName: RXPNM,
                        remarks: REMARKS,
                        amount: AMOUNT,
                    }
                }).success(function (data) {

                    if (data[0].TRANSNO != 0) {
                        var memoNO = data[0].TRANSNO;
                        $('#txt_TransNo').val(memoNO);
                        alert("Create Successfully !!");
                        //$scope.MediMasterData.push({ ID: data.ID, MCATID: data.MCATID, MCATNM: data.MCATNM });
                    } else {
                        alert("Please create again!");
                        window.location.reload(true);                      
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });
            }
            else {
                $('#txt_RxpPIDM').val("");
                $('#txt_PatientName').val("");
                alert("Please input patient id field !!");
            }
        }
    };


    // Complete Master Data
    $scope.complete = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var COMPID = $('#txtcompid').val();
        var INSUSERID = $('#txtInsertUserid').val();
        var INSLTUDE = $('#latlon').val();

        var TRANSNO = $('#txt_TransNo').val();
        var TRANSDT = $('#txt_TransDate').val();
        var RXPTP = $('#txt_RxpType').val();
        var RXPIDM = $('#txt_RxpPIDM').val();
        var NXTDT = $('#txt_NextDate').val();
        var RXPNM = $('#txt_PatientName').val();
        var REMARKS = $('#txt_Remarks').val();
        var Update_LogData = $('#txt_Update_LogData').val();
        var AMOUNT = $('#txt_Amount').val();

        if (TRANSNO != "" && RXPNM != "" && RXPIDM != "") {
            $http.get('/api/ApiPrescribeController/updateMasterData/', {
                params: {
                    Compid: COMPID,
                    insertUserID: INSUSERID,
                    insltude: INSLTUDE,
                    transno: TRANSNO,
                    rxpTp: RXPTP,
                    NextDate: NXTDT,
                    remarks: REMARKS,
                    patientName: RXPNM,
                    TransDate: TRANSDT,
                    UpLogData: Update_LogData,
                    amount: AMOUNT,
                }
            }).success(function (data) {

                if (data[0].TRANSNO != 0) {
                    alert("Complete Successfully !!");
                    window.location.reload(true);
                    event.preventDefault();
                    //$scope.MediMasterData.push({ ID: data.ID, MCATID: data.MCATID, MCATNM: data.MCATNM });
                } else {
                    window.location.reload(true);
                    alert("Please create again!");
                }

            }).error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });
        }
        else {
            $('#txt_RxpPIDM').val("");
            $('#txt_PatientName').val("");
            alert("Please input patient id field !!");
        }

    };







    //Add grid level data 1ST PART 
    $scope.addrow_1ST = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();

            this.newChild.PARTSL = $('#txt_1ST_Part').val();
            this.newChild.ENTRYTP = $('#EntryTypeID_1ST option:selected').val();
            this.newChild.GCATID = $('#txt_GCtegoryid_1ST').val();
            this.newChild.GHEADID = $('#txt_Hidden_GHEADID_1ST').val();
            this.newChild.RESULT = $('#Resutltid_1ST').val();
            this.newChild.GHEADEN = $('#DiagnosisNameID_1ST').val();

            this.newChild.RXPID = $('#Hiddentxt_RxPID').val();
            this.newChild.TRANSDT = $('#txt_TransDate').val();
            this.newChild.TRANSNO = $('#txt_TransNo').val();
            this.newChild.RXPTP = $('#txt_RxpType').val();
            this.newChild.RXPIDM = $('#txt_RxpPIDM').val();
            this.newChild.NXTDT = $('#txt_NextDate').val();
            this.newChild.RXPNM = $('#txt_PatientName').val();
            this.newChild.REMARKS = $('#txt_Remarks').val();

            if ((this.newChild.GHEADID != "" || this.newChild.RESULT != "") && this.newChild.TRANSNO != "" && this.newChild.RXPNM != "") {
                $http.post('/api/ApiPrescribeController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    var transno2 = $('#txt_TransNo').val();
                    $http.get('/api/ApiPrescribeController/Get1stPart/', {
                        params: {
                            Compid: compid2,
                            TrnsNo: transno2,
                        }
                    }).success(function (data) {
                        $scope.FirstPartData = data;
                        $scope.loading = false;
                    });


                    if (data.TRANSSL == "Not possible entry!!!") {
                        alert("Not possible entry!!!");
                    }
                    else if (data.TRANSSL != 0) {
                        $('#DiagnosisNameID_1ST').val("");
                        $('#txt_Hidden_GHEADID_1ST').val("");
                        $('#Resutltid_1ST').val("");
                        alert("Create Successfully !!");
                        //$scope.MediMasterData.push({ ID: data.ID, MCATID: data.MCATID, MCATNM: data.MCATNM });
                    } else {
                        $('#DiagnosisNameID_1ST').val("");
                        $('#txt_Hidden_GHEADID_1ST').val("");
                        $('#Resutltid_1ST').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else if (this.newChild.GHEADEN == "" || this.newChild.GHEADID == "" || this.newChild.RESULT == "") {
                $('#DiagnosisNameID_1ST').val("");
                $('#txt_Hidden_GHEADID_1ST').val("");
                $('#Resutltid_1ST').val("");
                alert("Diagnosis field required !!");
            }
            else {
                $('#DiagnosisNameID_1ST').val("");
                $('#txt_Hidden_GHEADID_1ST').val("");
                $('#Resutltid_1ST').val("");
                $('#txt_RxpPIDM').val("");
                $('#txt_PatientName').val("");
                alert("Please input patient id field !!");
            }
        }
    };



    //Delete grid level data 1ST PART
    $scope.deleteTestitem_1ST = function () {
        $scope.loading = true;
        var id = this.testitem.ID;

        var Delete = $('#txt_Deleteid').val();
        if (Delete == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.testitem.COMPID = $('#txtcompid').val();
            this.testitem.INSUSERID = $('#txtInsertUserid').val();
            this.testitem.INSLTUDE = $('#latlon').val();

            $http.post('/api/ApiPrescribeController/grid/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.FirstPartData, function (i) {
                    if ($scope.FirstPartData[i].ID === id) {
                        $scope.FirstPartData.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
                alert("Delete Successfully!!");

            }).error(function (data) {
                $scope.error = "An Error has occured while delete posts! " + data;
                $scope.loading = false;
            });
        }
    };












    //Add grid level data 2ND PART 
    $scope.addrow_2ND = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();

            this.newChild.PARTSL = $('#txt_2ND_Part').val();
            this.newChild.ENTRYTP = $('#EntryTypeID_2ND option:selected').val();
            this.newChild.GCATID = $('#txt_GCtegoryid_2ND').val();
            this.newChild.GHEADID = $('#txt_Hidden_GHEADID_2ND').val();
            this.newChild.RESULT = $('#Resutltid_2ND').val();
            this.newChild.GHEADEN = $('#HeadParticular_2ND').val();

            this.newChild.RXPID = $('#Hiddentxt_RxPID').val();
            this.newChild.TRANSDT = $('#txt_TransDate').val();
            this.newChild.TRANSNO = $('#txt_TransNo').val();
            this.newChild.RXPTP = $('#txt_RxpType').val();
            this.newChild.RXPIDM = $('#txt_RxpPIDM').val();
            this.newChild.NXTDT = $('#txt_NextDate').val();
            this.newChild.RXPNM = $('#txt_PatientName').val();
            this.newChild.REMARKS = $('#txt_Remarks').val();


            var changedtxt = $('#EntryTypeID_2ND').val(); // RECORD, MANUAL

            if (changedtxt == "MANUAL" && this.newChild.RESULT == "") {
                $('#txt_Hidden_GHEADID_2ND').val("");
                $('#Resutltid_2ND').val("");
                alert("Result field required !!");
            }
            else if (changedtxt == "RECORD" && this.newChild.GHEADID == "") {
                $('#HeadParticular_2ND').val("");
                alert("Please input valid Head particulars !!");
            }
            else if ((this.newChild.RESULT != "" || this.newChild.GHEADID != "") && this.newChild.TRANSNO != "" && this.newChild.RXPNM != "") {
                $http.post('/api/ApiPrescribeController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    var transno2 = $('#txt_TransNo').val();
                    $http.get('/api/ApiPrescribeController/Get2ndPart/', {
                        params: {
                            Compid: compid2,
                            TrnsNo: transno2,
                        }
                    }).success(function (data) {
                        $scope.SecondPartData = data;
                        $scope.loading = false;
                    });


                    if (data.TRANSSL == "Not possible entry!!!") {
                        alert("Not possible entry!!!");
                    }
                    else if (data.TRANSSL != 0) {
                        $('#HeadParticular_2ND').val("");
                        $('#txt_Hidden_GHEADID_2ND').val("");
                        $('#Resutltid_2ND').val("");
                        alert("Create Successfully !!");
                        //$scope.MediMasterData.push({ ID: data.ID, MCATID: data.MCATID, MCATNM: data.MCATNM });
                    } else {
                        $('#HeadParticular_2ND').val("");
                        $('#txt_Hidden_GHEADID_2ND').val("");
                        $('#Resutltid_2ND').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }

            else if (this.newChild.GHEADEN == "" && this.newChild.GHEADID == "" && this.newChild.RESULT == "") {
                $('#HeadParticular_2ND').val("");
                $('#txt_Hidden_GHEADID_2ND').val("");
                $('#Resutltid_2ND').val("");
                alert("grid level field required !!");
            }
            else {
                $('#HeadParticular_2ND').val("");
                $('#txt_Hidden_GHEADID_2ND').val("");
                $('#Resutltid_2ND').val("");
                $('#txt_RxpPIDM').val("");
                $('#txt_PatientName').val("");
                alert("Please input patient id field !!");
            }
        }
    };


    //Delete grid level data 2ND PART
    $scope.deleteTestitem_2ND = function () {
        $scope.loading = true;
        var id = this.testitem.ID;

        var Delete = $('#txt_Deleteid').val();
        if (Delete == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.testitem.COMPID = $('#txtcompid').val();
            this.testitem.INSUSERID = $('#txtInsertUserid').val();
            this.testitem.INSLTUDE = $('#latlon').val();

            $http.post('/api/ApiPrescribeController/grid/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.SecondPartData, function (i) {
                    if ($scope.SecondPartData[i].ID === id) {
                        $scope.SecondPartData.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
                alert("Delete Successfully!!");

            }).error(function (data) {
                $scope.error = "An Error has occured while delete posts! " + data;
                $scope.loading = false;
            });
        }
    };









    //Add grid level data 3RD PART 
    $scope.addrow_3RD = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();

            this.newChild.PARTSL = $('#txt_3RD_Part').val();
            this.newChild.ENTRYTP = $('#EntryTypeID_3RD option:selected').val();
            this.newChild.GCATID = $('#txt_GCtegoryid_3RD').val();
            this.newChild.GHEADID = $('#txt_Hidden_GHEADID_3RD').val();
            this.newChild.RESULT = $('#Resutltid_3RD').val();
            this.newChild.GHEADEN = $('#Investigation_3RD').val();

            this.newChild.RXPID = $('#Hiddentxt_RxPID').val();
            this.newChild.TRANSDT = $('#txt_TransDate').val();
            this.newChild.TRANSNO = $('#txt_TransNo').val();
            this.newChild.RXPTP = $('#txt_RxpType').val();
            this.newChild.RXPIDM = $('#txt_RxpPIDM').val();
            this.newChild.NXTDT = $('#txt_NextDate').val();
            this.newChild.RXPNM = $('#txt_PatientName').val();
            this.newChild.REMARKS = $('#txt_Remarks').val();

            if ((this.newChild.GHEADID != "" || this.newChild.RESULT != "") && this.newChild.TRANSNO != "" && this.newChild.RXPNM != "") {
                $http.post('/api/ApiPrescribeController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    var transno2 = $('#txt_TransNo').val();
                    $http.get('/api/ApiPrescribeController/Get3rdPart/', {
                        params: {
                            Compid: compid2,
                            TrnsNo: transno2,
                        }
                    }).success(function (data) {
                        $scope.ThirdPartData = data;
                        $scope.loading = false;
                    });


                    if (data.TRANSSL == "Not possible entry!!!") {
                        alert("Not possible entry!!!");
                    }
                    else if (data.TRANSSL != 0) {
                        $('#Investigation_3RD').val("");
                        $('#txt_GCtegoryid_3RD').val("");
                        $('#txt_Hidden_GHEADID_3RD').val("");
                        $('#Resutltid_3RD').val("");
                        alert("Create Successfully !!");
                        //$scope.MediMasterData.push({ ID: data.ID, MCATID: data.MCATID, MCATNM: data.MCATNM });
                    } else {
                        $('#Investigation_3RD').val("");
                        $('#txt_GCtegoryid_3RD').val("");
                        $('#txt_Hidden_GHEADID_3RD').val("");
                        $('#Resutltid_3RD').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else if (this.newChild.GHEADEN == "" || this.newChild.GHEADID == "" || this.newChild.RESULT == "") {
                $('#Investigation_3RD').val("");
                $('#txt_GCtegoryid_3RD').val("");
                $('#txt_Hidden_GHEADID_3RD').val("");
                $('#Resutltid_3RD').val("");
                alert("Diagnosis field required !!");
            }
            else {
                $('#Investigation_3RD').val("");
                $('#txt_GCtegoryid_3RD').val("");
                $('#txt_Hidden_GHEADID_3RD').val("");
                $('#Resutltid_3RD').val("");
                $('#txt_RxpPIDM').val("");
                $('#txt_PatientName').val("");
                alert("Please input patient id field !!");
            }
        }
    };



    //Delete grid level data 3RD PART
    $scope.deleteTestitem_3RD = function () {
        $scope.loading = true;
        var id = this.testitem.ID;

        var Delete = $('#txt_Deleteid').val();
        if (Delete == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.testitem.COMPID = $('#txtcompid').val();
            this.testitem.INSUSERID = $('#txtInsertUserid').val();
            this.testitem.INSLTUDE = $('#latlon').val();

            $http.post('/api/ApiPrescribeController/grid/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.ThirdPartData, function (i) {
                    if ($scope.ThirdPartData[i].ID === id) {
                        $scope.ThirdPartData.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
                alert("Delete Successfully!!");

            }).error(function (data) {
                $scope.error = "An Error has occured while delete posts! " + data;
                $scope.loading = false;
            });
        }
    };







    //Add grid level data 4TH PART 
    $scope.addrow_4TH = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();

            this.newChild.PARTSL = $('#txt_4TH_Part').val();
            this.newChild.ENTRYTP = $('#EntryTypeID_4TH option:selected').val();
            this.newChild.GCATID = $('#txt_GCtegoryid_4TH').val();
            this.newChild.GHEADID = $('#txt_Hidden_GHEADID_4TH').val();
            this.newChild.RESULT = $('#Resutltid_4TH').val();
            this.newChild.GHEADEN = $('#MedicineNameID_4TH').val();
            this.newChild.DOSEID = $('#txt_Hidden_DOSEID_4TH').val();
            this.newChild.DoseName = $('#DoseName_4TH').val();
            this.newChild.DDMMTP = $('#Time_4TH option:selected').val();
            this.newChild.DDMMQTY = $('#Qty_4TH').val();

            this.newChild.RXPID = $('#Hiddentxt_RxPID').val();
            this.newChild.TRANSDT = $('#txt_TransDate').val();
            this.newChild.TRANSNO = $('#txt_TransNo').val();
            this.newChild.RXPTP = $('#txt_RxpType').val();
            this.newChild.RXPIDM = $('#txt_RxpPIDM').val();
            this.newChild.NXTDT = $('#txt_NextDate').val();
            this.newChild.RXPNM = $('#txt_PatientName').val();
            this.newChild.REMARKS = $('#txt_Remarks').val();

            if ((this.newChild.GHEADID != "" || this.newChild.RESULT != "") && this.newChild.DDMMQTY != "" && this.newChild.TRANSNO != "" && this.newChild.RXPNM != "") {
                $http.post('/api/ApiPrescribeController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    var transno2 = $('#txt_TransNo').val();
                    $http.get('/api/ApiPrescribeController/Get4thPart/', {
                        params: {
                            Compid: compid2,
                            TrnsNo: transno2,
                        }
                    }).success(function (data) {
                        $scope.FourthPartData = data;
                        $scope.loading = false;
                    });


                    if (data.TRANSSL == "Not possible entry!!!") {
                        alert("Not possible entry!!!");
                    }
                    else if (data.TRANSSL != 0) {
                        $('#MedicineNameID_4TH').val("");
                        $('#txt_GCtegoryid_4TH').val("");
                        $('#txt_Hidden_GHEADID_4TH').val("");
                        $('#txt_Hidden_DOSEID_4TH').val("");
                        $('#Resutltid_4TH').val("");
                        $('#DoseName_4TH').val("");
                        $('#Qty_4TH').val("0");
                        alert("Create Successfully !!");
                        //$scope.MediMasterData.push({ ID: data.ID, MCATID: data.MCATID, MCATNM: data.MCATNM });
                    } else {
                        $('#MedicineNameID_4TH').val("");
                        $('#txt_GCtegoryid_4TH').val("");
                        $('#txt_Hidden_GHEADID_4TH').val("");
                        $('#txt_Hidden_DOSEID_4TH').val("");
                        $('#Resutltid_4TH').val("");
                        $('#DoseName_4TH').val("");
                        $('#Qty_4TH').val("0");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else if (this.newChild.DDMMQTY == "") {
                $('#Qty_4TH').val("0");
                alert("Qty filed required!!");
            }
            else if (this.newChild.GHEADEN == "" || this.newChild.GHEADID == "" || this.newChild.RESULT == "") {
                $('#MedicineNameID_4TH').val("");
                $('#txt_GCtegoryid_4TH').val("");
                $('#txt_Hidden_GHEADID_4TH').val("");
                $('#txt_Hidden_DOSEID_4TH').val("");
                $('#Resutltid_4TH').val("");
                $('#DoseName_4TH').val("");
                $('#Qty_4TH').val("0");
                alert("Medicine field required !!");
            }
            else {
                $('#MedicineNameID_4TH').val("");
                $('#txt_GCtegoryid_4TH').val("");
                $('#txt_Hidden_GHEADID_4TH').val("");
                $('#txt_Hidden_DOSEID_4TH').val("");
                $('#Resutltid_4TH').val("");
                $('#DoseName_4TH').val("");
                $('#Qty_4TH').val("0");
                $('#txt_RxpPIDM').val("");
                $('#txt_PatientName').val("");
                alert("Please input patient id field !!");
            }
        }
    };


    //Delete grid level data 4TH PART
    $scope.deleteTestitem_4TH = function () {
        $scope.loading = true;
        var id = this.testitem.ID;

        var Delete = $('#txt_Deleteid').val();
        if (Delete == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.testitem.COMPID = $('#txtcompid').val();
            this.testitem.INSUSERID = $('#txtInsertUserid').val();
            this.testitem.INSLTUDE = $('#latlon').val();

            $http.post('/api/ApiPrescribeController/grid/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.FourthPartData, function (i) {
                    if ($scope.FourthPartData[i].ID === id) {
                        $scope.FourthPartData.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
                alert("Delete Successfully!!");

            }).error(function (data) {
                $scope.error = "An Error has occured while delete posts! " + data;
                $scope.loading = false;
            });
        }
    };









    //Add grid level data 5TH PART 
    $scope.addrow_5TH = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();

            this.newChild.PARTSL = $('#txt_5TH_Part').val();
            this.newChild.ENTRYTP = $('#EntryTypeID_5TH option:selected').val();
            this.newChild.GCATID = $('#txt_GCtegoryid_5TH').val();
            this.newChild.GHEADID = $('#txt_Hidden_GHEADID_5TH').val();
            this.newChild.RESULT = $('#Resutltid_5TH').val();
            this.newChild.GHEADEN = $('#AdviceID_5TH').val();

            this.newChild.RXPID = $('#Hiddentxt_RxPID').val();
            this.newChild.TRANSDT = $('#txt_TransDate').val();
            this.newChild.TRANSNO = $('#txt_TransNo').val();
            this.newChild.RXPTP = $('#txt_RxpType').val();
            this.newChild.RXPIDM = $('#txt_RxpPIDM').val();
            this.newChild.NXTDT = $('#txt_NextDate').val();
            this.newChild.RXPNM = $('#txt_PatientName').val();
            this.newChild.REMARKS = $('#txt_Remarks').val();

            if ((this.newChild.GHEADID != "" || this.newChild.RESULT != "") && this.newChild.TRANSNO != "" && this.newChild.RXPNM != "") {
                $http.post('/api/ApiPrescribeController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    var transno2 = $('#txt_TransNo').val();
                    $http.get('/api/ApiPrescribeController/Get5thPart/', {
                        params: {
                            Compid: compid2,
                            TrnsNo: transno2,
                        }
                    }).success(function (data) {
                        $scope.FifthPartData = data;
                        $scope.loading = false;
                    });


                    if (data.TRANSSL == "Not possible entry!!!") {
                        alert("Not possible entry!!!");
                    }
                    else if (data.TRANSSL != 0) {
                        $('#AdviceID_5TH').val("");
                        $('#txt_Hidden_GHEADID_5TH').val("");
                        $('#Resutltid_5TH').val("");
                        alert("Create Successfully !!");
                        //$scope.MediMasterData.push({ ID: data.ID, MCATID: data.MCATID, MCATNM: data.MCATNM });
                    } else {
                        $('#AdviceID_5TH').val("");
                        $('#txt_Hidden_GHEADID_5TH').val("");
                        $('#Resutltid_5TH').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else if (this.newChild.GHEADEN == "" || this.newChild.GHEADID == "" || this.newChild.RESULT == "") {
                $('#AdviceID_5TH').val("");
                $('#txt_Hidden_GHEADID_5TH').val("");
                $('#Resutltid_5TH').val("");
                alert("Diagnosis field required !!");
            }
            else {
                $('#AdviceID_5TH').val("");
                $('#txt_Hidden_GHEADID_5TH').val("");
                $('#Resutltid_5TH').val("");
                $('#txt_RxpPIDM').val("");
                $('#txt_PatientName').val("");
                alert("Please input patient id field !!");
            }
        }
    };


    //Delete grid level data 5TH PART
    $scope.deleteTestitem_5TH = function () {
        $scope.loading = true;
        var id = this.testitem.ID;

        var Delete = $('#txt_Deleteid').val();
        if (Delete == "I") {
            alert("Permission Denied!!");
        }
        else {
            this.testitem.COMPID = $('#txtcompid').val();
            this.testitem.INSUSERID = $('#txtInsertUserid').val();
            this.testitem.INSLTUDE = $('#latlon').val();

            $http.post('/api/ApiPrescribeController/grid/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.FifthPartData, function (i) {
                    if ($scope.FifthPartData[i].ID === id) {
                        $scope.FifthPartData.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
                alert("Delete Successfully!!");

            }).error(function (data) {
                $scope.error = "An Error has occured while delete posts! " + data;
                $scope.loading = false;
            });
        }
    };


});