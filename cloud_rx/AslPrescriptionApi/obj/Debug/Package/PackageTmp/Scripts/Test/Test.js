var TestApp = angular.module("TestApp", ['ui.bootstrap']);


TestApp.controller("ApiTestController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;






    $scope.add = function (event) {

        $scope.loading = true;
        event.preventDefault();

        var compid = $('#txtcompid').val();
        var catid = $("#TestCatId").val();
        var catname = $('#TestCatName').val();
        var insltude = $('#latlon').val();
        var insertUserid = $('#txtInsertUserid').val();
        var insert = $('#txt_Insertid').val();

        $http.get('/api/ApiTestController/GetDataOrAddCategory/', {
            params: {
                Compid: compid,
                TCatID: catid,
                TCatName: catname,
                Insltude: insltude,
                loggedUserID: insertUserid,
                Insert_Permission: insert,
            }
        }).success(function (data) {
            var insertPermission = data[0].Insert;
            if (insertPermission == "I") {
                $('#TestCatName').val("");
                $scope.TestMasterData = null;
                alert("Permission Denied!!");
            }
            else {
                var TCatID = data[0].TCATID;
                var TestID = data[0].TESTID;
                var TCatName = data[0].TCATNM;
                var insertNewDataShowMessage = data[0].InsertNewDataShowMessage;
                $('#TestCatId').val(TCatID);
                if (TCatID != 0 && TestID != 0 && TestID != null) {
                    $scope.TestMasterData = data;
                }
                else if (insertNewDataShowMessage != null) {
                    $scope.TestMasterData = null;
                    alert("Category name insert successfully!");
                } else {
                    $scope.TestMasterData = null;
                }

                if (TCatName == null) {
                    $scope.TestMasterData = null;
                    $('#TestCatName').val("");
                    alert("Category Name can not be empty!");
                }
                $scope.loading = false;
            }
        });

    };









    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;

    };
    



    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;
        //  Grid view load 
        var compid = $('#txtcompid').val();
        var catid = $("#TestCatId").val();
        var catname = $('#TestCatName').val();
        var insltude = $('#latlon').val();
        var insertUserid = $('#txtInsertUserid').val();
        var insert = $('#txt_Insertid').val();

        $http.get('/api/ApiTestController/GetDataOrAddCategory/', {
            params: {
                Compid: compid,
                TCatID: catid,
                TCatName: catname,
                Insltude: insltude,
                loggedUserID: insertUserid,
                Insert_Permission: insert,
            }
        }).success(function (data) {
            var insertPermission = data[0].Insert;
            if (insertPermission == "I") {
                $('#TestCatName').val("");
                $scope.TestMasterData = null;
                alert("Permission Denied!!");
            }
            else {
                var TCatID = data[0].TCATID;
                var TestID = data[0].TESTID;
                $('#TestCatId').val(TCatID);
                if (TCatID != 0 && TestID != 0 && TestID != null) {
                    $scope.TestMasterData = data;
                }
                else {
                    $scope.TestMasterData = null;
                }
                $scope.loading = false;
            }
        });
    };
    

    //$scope.toggleAdd = function () {
    //    $scope.addMode = !$scope.addMode;
    //};




    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            $('#testnm').val("");
            alert("Permission Denied!!");         
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.TCATNM = $('#TestCatName').val();
            this.newChild.TCATID = $('#TestCatId').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();
            this.newChild.TESTNM = $('#testnm').val();

            if (this.newChild.TCATID != 0 && this.newChild.TCATID != "" && this.newChild.TESTNM != "" && this.newChild.TCATNM != "") {
                $http.post('/api/ApiTestController/grid/TestChild', this.newChild).success(function (data, status, headers, config) {
                    
                    var compid = $('#txtcompid').val();
                    var catid = $("#TestCatId").val();
                    var catname = $('#TestCatName').val();
                    var insltude = $('#latlon').val();
                    var insertUserid = $('#txtInsertUserid').val();
                    var insert1 = $('#txt_Insertid').val();
                    
                    $http.get('/api/ApiTestController/GetDataOrAddCategory/', {
                        params: {
                            Compid: compid,
                            TCatID: catid,
                            TCatName: catname,
                            Insltude: insltude,
                            loggedUserID: insertUserid,
                            Insert_Permission: insert1,
                        }
                    }).success(function (data) {
                        var TCatID = data[0].TCATID;
                        //var TestID = data[0].TESTID;
                        $('#TestCatId').val(TCatID);
                        //if (TCatID != 0 && TestID != 0) {
                        if (TCatID != 0) {
                            $scope.TestMasterData = data;
                        }
                        else {
                            $scope.TestMasterData = null;
                            alert("Please input the category name first!");
                        }

                        $scope.loading = false;

                    });


                    if (data.TESTID != 0) {
                        $('#testnm').val("");
                        alert("Create Successfully !!");
                        //$scope.TestMasterData.push({ ID: data.ID, TCATID: data.TCATID, TESTID: data.TESTID, TESTNM: data.TESTNM });
                    } else {
                        $('#testnm').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            } else if (this.newChild.TCATNM == "" || this.newChild.TCATNM == null) {
                $('#testnm').val("");
                alert("Please input Category name field !!");
            } else if (this.newChild.TCATID == 0 || this.newChild.TCATID == "") {
                $('#testnm').val("");
                alert("Please add Category name First !!");
            } else {
                $('#testnm').val("");
                alert("Please input test name field !!");
            }
        }
    };





    //Update to grid level data (save a record after edit)
    $scope.save = function () {
        // alert("Edit");
        $scope.loading = true;
        var frien = this.testitem;
        
        this.testitem.COMPID = $('#txtcompid').val();
        this.testitem.INSUSERID = $('#txtInsertUserid').val();
        this.testitem.INSLTUDE = $('#latlon').val();
        var Update = $('#txt_Updateid').val();

        if (Update == "I") {
            alert("Permission Denied!!");
            frien.editMode = false;
            var compid = $('#txtcompid').val();
            var catid = $("#TestCatId").val();
            var catname = $('#TestCatName').val();
            var insltude = $('#latlon').val();
            var insertUserid = $('#txtInsertUserid').val();
            var insert = $('#txt_Insertid').val();
            
            $http.get('/api/ApiTestController/GetDataOrAddCategory/', {
                params: {
                    Compid: compid,
                    TCatID: catid,
                    TCatName: catname,
                    Insltude: insltude,
                    loggedUserID: insertUserid,
                    Insert_Permission: insert,
                }
            }).success(function (data) {
                var TCatID = data[0].TCATID;
                //var TestID = data[0].TESTID;

                $('#TestCatId').val(TCatID);

                //if (TCatID != 0 && TestID != 0) {
                if (TCatID != 0) {
                    $scope.TestMasterData = data;
                }
                else {
                    $scope.TestMasterData = null;
                }
            });

            $scope.loading = false;          
        }
        else {
            $http.post('/api/ApiTestController/grid/UpdateChildData', this.testitem).success(function (data) {
                if (data.TESTID != 0) {
                    alert("Saved Successfully!!");
                } else {
                    alert("Duplicate data entered will not create!");
                }

                frien.editMode = false;

                var compid1 = $('#txtcompid').val();
                var catid1 = $("#TestCatId").val();
                var catname1 = $('#TestCatName').val();
                var insltude1 = $('#latlon').val();
                var insertUserid1 = $('#txtInsertUserid').val();
                var insert1 = $('#txt_Insertid').val();

                $http.get('/api/ApiTestController/GetDataOrAddCategory/', {
                    params: {
                        Compid: compid1,
                        TCatID: catid1,
                        TCatName: catname1,
                        Insltude: insltude1,
                        loggedUserID: insertUserid1,
                        Insert_Permission: insert1,
                    }
                }).success(function (data) {
                    var TCatID = data[0].TCATID;
                    //var TestID = data[0].TESTID;

                    $('#TestCatId').val(TCatID);

                    //if (TCatID != 0 && TestID != 0) {
                    if (TCatID != 0) {
                        $scope.TestMasterData = data;
                    }
                    else {
                        $scope.TestMasterData = null;
                    }
                });

                $scope.loading = false;
            }).error(function (data) {
                $scope.error = "An Error has occured while Saving Friend! " + data;
                $scope.loading = false;

            });
        }     
    };







    //Delete grid level data.
    $scope.deleteTestitem = function () {
        $scope.loading = true;
        var id = this.testitem.ID;
        
        this.testitem.COMPID = $('#txtcompid').val();
        this.testitem.INSUSERID = $('#txtInsertUserid').val();
        this.testitem.INSLTUDE = $('#latlon').val();
        var Delete = $('#txt_Deleteid').val();

        if (Delete == "I") {
            alert("Permission Denied!!");
        }
        else {
            $http.post('/api/ApiTestController/grid/DeleteChildData/', this.testitem).success(function (data) {

                $.each($scope.TestMasterData, function (i) {
                    if ($scope.TestMasterData[i].ID === id) {
                        $scope.TestMasterData.splice(i, 1);
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
