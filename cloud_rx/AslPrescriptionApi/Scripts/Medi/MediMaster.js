var MediMasterApp = angular.module("MediMasterApp", ['ui.bootstrap']);

MediMasterApp.controller("ApiMediMasterController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;
    

    // Inital Grid view load 
    var compid = $('#txtcompid').val();
    $http.get('/api/ApiMediMasterController/GetMediMasterInfo/', {
        params: {
            Compid: compid,
        }
    }).success(function (data) {
        if (data[0].count == 1) {
            $scope.MediMasterData = null;
        } else {
            $scope.MediMasterData = data;
        }
        $scope.loading = false;
    });
   



    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };



    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;
        //Grid view load 
        var compid1 = $('#txtcompid').val();
        $http.get('/api/ApiMediMasterController/GetMediMasterInfo/', {
            params: {
                Compid: compid1,
            }
        }).success(function (data) {
            $scope.MediMasterData = data;
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
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();
            this.newChild.MCATNM = $('#MediNameID').val();
            if (this.newChild.MCATNM != "") {
                $http.post('/api/ApiMediMasterController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    $http.get('/api/ApiMediMasterController/GetMediMasterInfo/', {
                        params: {
                            Compid: compid2,
                        }
                    }).success(function (data) {
                        $scope.MediMasterData = data;
                        $scope.loading = false;
                    });


                    if (data.MCATID != 0) {
                        $('#MediNameID').val("");
                        alert("Create Successfully !!");
                        //$scope.MediMasterData.push({ ID: data.ID, MCATID: data.MCATID, MCATNM: data.MCATNM });
                    } else {
                        $('#MediNameID').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else {
                $('#MediNameID').val("");
                alert("Please input pharma name field !!");
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
        var Update = $('#txt_Updateid').val();

        if (Update == "I") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var compid3 = $('#txtcompid').val();
            $http.get('/api/ApiMediMasterController/GetMediMasterInfo/', {
                params: {
                    Compid: compid3,
                }
            }).success(function (data) {
                $scope.MediMasterData = data;
            });

            $scope.loading = false;
        }
        else {
            $http.post('/api/ApiMediMasterController/grid/UpdateData', this.testitem).success(function (data) {
                if (data.MCATID != 0) {
                    alert("Saved Successfully!!");
                } else {
                    alert("Duplicate data entered will not create!");
                }

                frien.editMode = false;

                //Grid view load 
                var compid4 = $('#txtcompid').val();
                $http.get('/api/ApiMediMasterController/GetMediMasterInfo/', {
                    params: {
                        Compid: compid4,
                    }
                }).success(function (data) {
                    $scope.MediMasterData = data;
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

            $http.post('/api/ApiMediMasterController/grid/DeleteData/', this.testitem).success(function (data) {

                var getChildDataForDeleteMasterCategory = data.GetChildDataForDeleteMasterCategory;
                if (getChildDataForDeleteMasterCategory == 1) {
                    $scope.loading = false;
                    alert("Please firstly delete category wise child data first!!");
                }
                else {
                    $.each($scope.MediMasterData, function (i) {
                        if ($scope.MediMasterData[i].ID === id) {
                            $scope.MediMasterData.splice(i, 1);
                            return false;
                        }
                    });
                    $scope.loading = false;
                    alert("Delete Successfully!!");
                }
            }).error(function (data) {
                $scope.error = "An Error has occured while delete posts! " + data;
                $scope.loading = false;
            });
        }
    };
    

});