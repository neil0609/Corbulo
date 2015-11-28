sabio.page.officeHoursListControllerFactory = function (
    $scope
    , $baseController
    , $officeHoursService
    , $routeParams
    , $utilityService
    , $route
    ) {

    var vm = this;

    $baseController.merge(vm, $baseController);

    vm.headingInfo = "Awesome!";

    vm.$scope = $scope;
    vm.$officeHoursService = $officeHoursService;
    vm.$routeParams = $routeParams;
    vm.$utilityService = $utilityService;
    vm.$route = $route;

    vm.onGetOfficeHourListSuccess = _onGetOfficeHourListSuccess;
    vm.onGetOfficeHourListError = _onGetOfficeHourListError;

    vm.deleteOfficeHourSession = _deleteOfficeHourSession;
    vm.onDeleteOfficeHourSessionSuccess = _onDeleteOfficeHourSessionSuccess;
    vm.onDeleteOfficeHourSessionError = _onDeleteOfficeHourSessionError;

    //vm.convertTimeDateToDateObject = _convertTimeDateToDateObject;

    vm.notify = vm.$officeHoursService.getNotifier($scope);

    render();

    function render() {
        vm.$officeHoursService.getOfficeHourList(vm.onGetOfficeHourListSuccess, vm.onGetOfficeHourListError);
    }


    function _onGetOfficeHourListSuccess(data) {
        //this receives the data and calls the special
        //notify method that will trigger ng to refresh UI
        vm.notify(function () {
            vm.schedule = data.items;
            //vm.convertTimeDateToDateObject();
            console.log(vm.schedule);
        });
    }

    //function _convertTimeDateToDateObject() {
    //    vm.session.startTime = vm.$utilityService.dateFromMilitaryTime(vm.session.startTime);
    //    vm.session.endTime = vm.$utilityService.dateFromMilitaryTime(vm.session.endTime);

    //    if (vm.session.date)
    //        vm.session.date = new Date(vm.session.date);
    //}

    function _onGetOfficeHourListError(jqXhr, error) {
        console.log(error);
    }

    function _deleteOfficeHourSession(id) {
        vm.$officeHoursService.deleteOfficeHourSession(id, vm.onDeleteOfficeHourSessionSuccess, vm.onDeleteOfficeHourSessionError);
        vm.$route.reload();
    }
   
    function _onDeleteOfficeHourSessionSuccess(data) {
        //this receives the data and calls the special
        //notify method that will trigger ng to refresh UI
        vm.notify(function () {
           
            console.log("schedule deleted", data);
        });
    }

    function _onDeleteOfficeHourSessionError(jqXhr, error) {
        console.log(error);
    }


}


sabio.ng.addController(sabio.ng.app.module
, "officeHoursListController"
, ['$scope', '$baseController', '$officeHoursService', '$routeParams','$utilityService', '$route']
, sabio.page.officeHoursListControllerFactory);



