var GheadMasterApp = angular.module("GheadMasterApp", ['ui.bootstrap']);

GheadMasterApp.controller("ApiGheadMasterController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    // Inital Grid view load 
    var compid = $('#txtcompid').val();
    $http.get('/api/ApiGheadMasterController/GetGheadMasterInfo/', {
        params: {
            Compid: compid,
        }
    }).success(function (data) {
        if (data[0].count == 1) {
            $scope.GheadMasterData = null;
        } else {
            $scope.GheadMasterData = data;
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
        $http.get('/api/ApiGheadMasterController/GetGheadMasterInfo/', {
            params: {
                Compid: compid1,
            }
        }).success(function (data) {
            $scope.GheadMasterData = data;
            $scope.loading = false;
        });
    };





    //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            $('#GheadNameID').val("");
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();
            this.newChild.GCATNM = $('#GheadNameID').val();
            if (this.newChild.GCATNM != "") {
                $http.post('/api/ApiGheadMasterController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    $http.get('/api/ApiGheadMasterController/GetGheadMasterInfo/', {
                        params: {
                            Compid: compid2,
                        }
                    }).success(function (data) {
                        $scope.GheadMasterData = data;
                        $scope.loading = false;
                    });


                    if (data.GCATID != 0) {
                        $('#GheadNameID').val("");
                        alert("Create Successfully !!");
                       // $scope.GheadMasterData.push({ ID: data.ID, GCATID: data.GCATID, GCATNM: data.GCATNM });
                    } else {
                        $('#GheadNameID').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else {
                $('#GheadNameID').val("");
                alert("Please input category name field !!");
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
            $http.get('/api/ApiGheadMasterController/GetGheadMasterInfo/', {
                params: {
                    Compid: compid3,
                }
            }).success(function (data) {
                $scope.GheadMasterData = data;
            });

            $scope.loading = false;
        }
        else {
            $http.post('/api/ApiGheadMasterController/grid/UpdateData', this.testitem).success(function (data) {
                if (data.GCATID != 0) {
                    alert("Saved Successfully!!");
                } else {
                    alert("Duplicate data will not create!");
                }

                frien.editMode = false;

                //Grid view load 
                var compid4 = $('#txtcompid').val();
                $http.get('/api/ApiGheadMasterController/GetGheadMasterInfo/', {
                    params: {
                        Compid: compid4,
                    }
                }).success(function (data) {
                    $scope.GheadMasterData = data;
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

            $http.post('/api/ApiGheadMasterController/grid/DeleteData/', this.testitem).success(function (data) {

                var getChildDataForDeleteMasterCategory = data.GetChildDataForDeleteMasterCategory;
                if (getChildDataForDeleteMasterCategory == 1) {
                    $scope.loading = false;
                    alert("Please firstly delete category wise child data first!!");
                }
                else {
                    $.each($scope.GheadMasterData, function (i) {
                        if ($scope.GheadMasterData[i].ID === id) {
                            $scope.GheadMasterData.splice(i, 1);
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