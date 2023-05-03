var PharmaApp = angular.module("PharmaApp", ['ui.bootstrap']);

PharmaApp.controller("ApiPharmaController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;
    
    


    // Inital Grid view load 
    var compid = $('#txtcompid').val();
    $http.get('/api/ApiPharmaController/GetPharma/', {
        params: {
            Compid: compid,
        }
    }).success(function (data) {
        if (data[0].count == 1) {
            $scope.PharmaData = null;
        } else {
            $scope.PharmaData = data;
        }
        $scope.loading = false;
    });




    
    $scope.toggleEdit = function () {
        this.testitem.editMode = !this.testitem.editMode;
    };
    
    
    $scope.toggleEdit_Cancel = function () {
        this.testitem.editMode = !this.testitem.editMode;
        //Grid view load 
        var compid3 = $('#txtcompid').val();
        $http.get('/api/ApiPharmaController/GetPharma/', {
            params: {
                Compid: compid3,
            }
        }).success(function (data) {
            $scope.PharmaData = data;
            $scope.loading = false;
        });
    };
    




     //Add grid level data
    $scope.addrow = function (event) {
        $scope.loading = false;
        event.preventDefault();

        var insert = $('#txt_Insertid').val();
        if (insert == "I") {
            $('#pharmaNameID').val("");
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();
            this.newChild.PHARMANM = $('#pharmaNameID').val();
            this.newChild.STATUS = $('#statusID').val();
            if (this.newChild.PHARMANM != "") {
                $http.post('/api/ApiPharmaController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid1 = $('#txtcompid').val();
                    $http.get('/api/ApiPharmaController/GetPharma/', {
                        params: {
                            Compid: compid1,
                        }
                    }).success(function (data) {
                        $scope.PharmaData = data;
                        $scope.loading = false;
                    });


                    if (data.PHARMAID != 0) {
                        $('#pharmaNameID').val("");
                        alert("Create Successfully !!");
                        //$scope.PharmaData.push({ ID: data.ID, PHARMAID: data.PHARMAID, PHARMANM: data.PHARMANM, STATUS: data.STATUS });
                    } else {
                        $('#pharmaNameID').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else {
                $('#pharmaNameID').val("");
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
        //this.testitem.STATUS = $("#gridStatusID option:selected").val();
        var Update = $('#txt_Updateid').val();

        if (Update == "I") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var compid5 = $('#txtcompid').val();
            $http.get('/api/ApiPharmaController/GetPharma/', {
                params: {
                    Compid: compid5,
                }
            }).success(function (data) {
                $scope.PharmaData = data;
            });
            
            $scope.loading = false;
        }
        else {
            $http.post('/api/ApiPharmaController/grid/UpdateData', this.testitem).success(function (data) {
                if (data.PHARMAID != 0) {
                    alert("Saved Successfully!!");
                } else {
                    alert("Duplicate data entered will not create!");
                }

                frien.editMode = false;

                //Grid view load 
                var compid6 = $('#txtcompid').val();
                $http.get('/api/ApiPharmaController/GetPharma/', {
                    params: {
                        Compid: compid6,
                    }
                }).success(function (data) {
                    $scope.PharmaData = data;
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
            
            $http.post('/api/ApiPharmaController/grid/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.PharmaData, function (i) {
                    if ($scope.PharmaData[i].ID === id) {
                        $scope.PharmaData.splice(i, 1);
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