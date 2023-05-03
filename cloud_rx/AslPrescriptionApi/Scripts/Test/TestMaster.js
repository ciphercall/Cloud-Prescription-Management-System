var TestMasterApp = angular.module("TestMasterApp", ['ui.bootstrap']);

TestMasterApp.controller("ApiTestMasterController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;



    // Inital Grid view load 
    var compid = $('#txtcompid').val();
    var insltude = $('#latlon').val();
    var insertUserid = $('#txtInsertUserid').val();
    $http.get('/api/ApiTestMasterController/GetCategory/', {
        params: {
            Compid: compid,
            Insltude: insltude,
            loggedUserID: insertUserid,
        }
    }).success(function (data) {
        $scope.TestMasterData = data;
        $scope.loading = false;
    });






    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };


    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;
        //Grid view load 
        var compid4 = $('#txtcompid').val();
        var insltude4 = $('#latlon').val();
        var insertUserid4 = $('#txtInsertUserid').val();
        $http.get('/api/ApiTestMasterController/GetCategory/', {
            params: {
                Compid: compid4,
                Insltude: insltude4,
                loggedUserID: insertUserid4,
            }
        }).success(function (data) {
            $scope.TestMasterData = data;
            $scope.loading = false;
        });
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

            var compid1 = $('#txtcompid').val();
            var insltude1 = $('#latlon').val();
            var insertUserid1 = $('#txtInsertUserid').val();

            $http.get('/api/ApiTestMasterController/GetCategory/', {
                params: {
                    Compid: compid1,
                    Insltude: insltude1,
                    loggedUserID: insertUserid1,
                }
            }).success(function (data) {
                $scope.TestMasterData = data;                
            });
            $scope.loading = false;
           
        }
        else {
            $http.post('/api/ApiTestMasterController/grid/UpdateData', this.testitem).success(function (data) {
                if (data.TESTID != 0) {
                    alert("Saved Successfully!!");
                } else {
                    alert("Duplicate data entered will not create!");
                }

                frien.editMode = false;

                var compid2 = $('#txtcompid').val();
                var insltude2 = $('#latlon').val();
                var insertUserid2 = $('#txtInsertUserid').val();

                $http.get('/api/ApiTestMasterController/GetCategory/', {
                    params: {
                        Compid: compid2,
                        Insltude: insltude2,
                        loggedUserID: insertUserid2,
                    }
                }).success(function (data) {
                    $scope.TestMasterData = data;
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
        
        this.testitem.COMPID = $('#txtcompid').val();
        this.testitem.INSUSERID = $('#txtInsertUserid').val();
        this.testitem.INSLTUDE = $('#latlon').val();
        this.testitem.Delete = $('#txt_Deleteid').val();

        if (this.testitem.Delete == "I") {
            alert("Permission Denied!!");
        }
        else {
            $http.post('/api/ApiTestMasterController/grid/DeleteData/', this.testitem).success(function (data) {

                var getChildDataForDeleteMasterCategory = data.GetChildDataForDeleteMasterCategory;
                if (getChildDataForDeleteMasterCategory == 1) {
                    $scope.loading = false;
                    alert("Please firstly delete category wise child data first!!");
                }
                else {
                    $.each($scope.TestMasterData, function (i) {
                        if ($scope.TestMasterData[i].ID === id) {
                            $scope.TestMasterData.splice(i, 1);
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