var MediCareApp = angular.module("MediCareApp", ['ui.bootstrap']);

MediCareApp.controller("ApiMediCareController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {

        $scope.loading = true;
        event.preventDefault();

        var compid = $('#txtcompid').val();
        var changedDDCategoryId = $('#ddlMCatID option:selected').val();

        $http.get('/api/ApiMediCareController/GetMediCareData/', {
            params: {
                Compid: compid,
                MCatid: changedDDCategoryId,
            }
        }).success(function (data) {
            if (data[0].count == 1) {
                $scope.MediCareData = null;
            } else {
                $scope.MediCareData = data;
            }
            $scope.loading = false;
        });

    };




    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };



    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;

        //Grid view load 
        var compid = $('#txtcompid').val();
        var changedDDCategoryId = $('#ddlMCatID option:selected').val();

        $http.get('/api/ApiMediCareController/GetMediCareData/', {
            params: {
                Compid: compid,
                MCatid: changedDDCategoryId,
            }
        }).success(function (data) {
            $scope.MediCareData = data;
            $scope.loading = false;
        });
    };




    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            $('#MediNameID').val("");
            $('#pharmaNameID').val("");
            $('#GheadNameID').val("");
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();
            this.newChild.MEDINM = $('#MediNameID').val(); 
            this.newChild.PHARMAID = $('#pharmaNameID option:selected').val();
            this.newChild.GHEADID = $('#GheadNameID option:selected').val();
            this.newChild.MCATID = $('#ddlMCatID option:selected').val();
            
            if ((this.newChild.MCATID != "" || this.newChild.MCATID != 0) && this.newChild.MEDINM != "" && this.newChild.PHARMAID != "" ) {
                $http.post('/api/ApiMediCareController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    var changedDDCategoryId2 = $('#ddlMCatID option:selected').val();

                    $http.get('/api/ApiMediCareController/GetMediCareData/', {
                        params: {
                            Compid: compid2,
                            MCatid: changedDDCategoryId2,
                        }
                    }).success(function (data) {
                        $scope.MediCareData = data;
                        $scope.loading = false;
                    });


                    if (data.MEDIID != 0) {
                        $('#MediNameID').val("");
                        $('#pharmaNameID').val("");
                        $('#GheadNameID').val("");
                        alert("Create Successfully !!");
                        //$scope.MediCareData.push({ ID: data.ID, MCATID: data.MCATID, MEDIID: data.MEDIID, MEDINM: data.MEDINM, PHARMAID: data.PHARMAID, GHEADID: data.GHEADID });
                    } else {
                        $('#MediNameID').val("");
                        $('#pharmaNameID').val("");
                        $('#GheadNameID').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else if (this.newChild.MCATID == 0 || this.newChild.MCATID == "") {
                $('#ddlMCatID').val("");
                $('#MediNameID').val("");
                $('#pharmaNameID').val("");
                $('#GheadNameID').val("");
                alert("Please select category name first !!");
            }
            else if (this.newChild.MEDINM == "") {
                $('#MediNameID').val("");
                alert("Please input medical name field !!");
            }
            else if (this.newChild.PHARMAID == "") {
                $('#pharmaNameID').val("");
                alert("Please select pharma name field !!");
            }
            else {
                alert("Please input grid level data !!");
            }

        }
    };








    //Update to grid level data (save a record after edit)
    $scope.update = function () {
        $scope.loading = true;
        var frien = this.testitem;

        this.testitem.COMPID = $('#txtcompid').val();
        this.testitem.INSUSERID = $('#txtInsertUserid').val();
        this.testitem.INSLTUDE = $('#latlon').val();
        this.testitem.MCATID = $('#ddlMCatID option:selected').val();
        var Update = $('#txt_Updateid').val();

        if (Update == "I") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var compid2 = $('#txtcompid').val();
            var changedDDCategoryId2 = $('#ddlMCatID option:selected').val();

            $http.get('/api/ApiMediCareController/GetMediCareData/', {
                params: {
                    Compid: compid2,
                    MCatid: changedDDCategoryId2,
                }
            }).success(function (data) {
                $scope.MediCareData = data;
            });

            $scope.loading = false;
        }
        else {
            $http.post('/api/ApiMediCareController/grid/UpdateData', this.testitem).success(function (data) {
                if (data.MEDIID != 0) {
                    alert("Saved Successfully!!");
                } else {
                    alert("Duplicate data will not create!");
                }

                frien.editMode = false;

                //Grid view load 
                var compid3 = $('#txtcompid').val();
                var changedDDCategoryId3 = $('#ddlMCatID option:selected').val();

                $http.get('/api/ApiMediCareController/GetMediCareData/', {
                    params: {
                        Compid: compid3,
                        MCatid: changedDDCategoryId3,
                    }
                }).success(function (data) {
                    $scope.MediCareData = data;
                });
                $scope.loading = false;

            }).error(function (data) {
                $scope.error = "An Error has occured while Saving Friend! " + data;
                $scope.loading = false;

            });
        }
    };






    //Delete grid level data.
    $scope.deleteItem = function () {
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

            $http.post('/api/ApiMediCareController/grid/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.MediCareData, function (i) {
                    if ($scope.MediCareData[i].ID === id) {
                        $scope.MediCareData.splice(i, 1);
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