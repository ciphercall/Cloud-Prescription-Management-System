var GheadApp = angular.module("GheadApp", ['ui.bootstrap']);

GheadApp.controller("ApiGheadController", function ($scope, $http) {

    $scope.loading = false;
    $scope.addMode = false;


    $scope.search = function (event) {

        $scope.loading = true;
        event.preventDefault();

        var compid = $('#txtcompid').val();
        var changedDDCategoryId = $('#ddlgcatID option:selected').val();

        $http.get('/api/ApiGheadController/GetCategoryWiseHeadInfo/', {
            params: {
                Compid: compid,
                GCatid: changedDDCategoryId,
            }
        }).success(function (data) {
            if (data[0].count == 1) {
                $scope.GheadData = null;
            } else {
                $scope.GheadData = data;
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
        var changedDDCategoryId = $('#ddlgcatID option:selected').val();

        $http.get('/api/ApiGheadController/GetCategoryWiseHeadInfo/', {
            params: {
                Compid: compid,
                GCatid: changedDDCategoryId,
            }
        }).success(function (data) {
            $scope.GheadData = data;
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
            $('#GheadbgID').val("");
            alert("Permission Denied!!");
        }
        else {
            this.newChild.COMPID = $('#txtcompid').val();
            this.newChild.INSUSERID = $('#txtInsertUserid').val();
            this.newChild.INSLTUDE = $('#latlon').val();
            this.newChild.GHEADEN = $('#GheadNameID').val();
            this.newChild.GHEADBG = $('#GheadbgID').val();
            this.newChild.SHOWTP = $('#ShowTypeID').val();
            this.newChild.GCATID = $('#ddlgcatID option:selected').val();

            if ((this.newChild.GCATID != "" || this.newChild.GCATID != 0) && this.newChild.GHEADEN != "") {
                $http.post('/api/ApiGheadController/grid/addData', this.newChild).success(function (data, status, headers, config) {

                    //Grid view load 
                    var compid2 = $('#txtcompid').val();
                    var changedDDCategoryId2 = $('#ddlgcatID option:selected').val();

                    $http.get('/api/ApiGheadController/GetCategoryWiseHeadInfo/', {
                        params: {
                            Compid: compid2,
                            GCatid: changedDDCategoryId2,
                        }
                    }).success(function (data) {
                        $scope.GheadData = data;
                        $scope.loading = false;
                    });


                    if (data.GHEADID != 0) {
                        $('#GheadNameID').val("");
                        $('#GheadbgID').val("");
                        alert("Create Successfully !!");
                        //$scope.GheadData.push({ ID: data.ID, GCATID: data.GCATID, GHEADID: data.GHEADID, GHEADEN: data.GHEADEN, GHEADBG: data.GHEADBG });
                    } else {
                        $('#GheadNameID').val("");
                        $('#GheadbgID').val("");
                        alert("Duplicate name will not create!");
                    }

                }).error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });

            }
            else if (this.newChild.GCATID == 0 || this.newChild.GCATID == "") {
                $('#ddlgcatID').val("");
                alert("Please select Category name First !!");
            }
            else if (this.newChild.GHEADEN == "") {
                $('#GheadNameID').val("");
                alert("Please input particulars(English) name field !!");
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
        this.testitem.GCATID = $('#ddlgcatID option:selected').val();
        var Update = $('#txt_Updateid').val();

        if (Update == "I") {
            alert("Permission Denied!!");
            frien.editMode = false;

            //Grid view load 
            var compid2 = $('#txtcompid').val();
            var changedDDCategoryId2 = $('#ddlgcatID option:selected').val();

            $http.get('/api/ApiGheadController/GetCategoryWiseHeadInfo/', {
                params: {
                    Compid: compid2,
                    GCatid: changedDDCategoryId2,
                }
            }).success(function (data) {
                $scope.GheadData = data;
            });

            $scope.loading = false;
        }
        else {
            $http.post('/api/ApiGheadController/grid/UpdateData', this.testitem).success(function (data) {
                if (data.GHEADID != 0) {
                    alert("Saved Successfully!!");
                } else {
                    alert("Duplicate data will not create!");
                }

                frien.editMode = false;

                //Grid view load 
                var compid3 = $('#txtcompid').val();
                var changedDDCategoryId3 = $('#ddlgcatID option:selected').val();

                $http.get('/api/ApiGheadController/GetCategoryWiseHeadInfo/', {
                    params: {
                        Compid: compid3,
                        GCatid: changedDDCategoryId3,
                    }
                }).success(function (data) {
                    $scope.GheadData = data;
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

            $http.post('/api/ApiGheadController/grid/DeleteData/', this.testitem).success(function (data) {

                $.each($scope.GheadData, function (i) {
                    if ($scope.GheadData[i].ID === id) {
                        $scope.GheadData.splice(i, 1);
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