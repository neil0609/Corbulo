sabio.page.officeHoursManagerControllerFactory = function (
    $scope
    , $baseController
    , $officeHoursService
    , $routeParams
    , $location
    , $route
    , $templateCache
    , $utilityService
    ) {

    var vm = this;

    $baseController.merge(vm, $baseController);

    vm.headingInfo = "Awesome!";

    vm.$scope = $scope;
    vm.$officeHoursService = $officeHoursService;
    vm.$routeParams = $routeParams;
    vm.$location = $location;
    vm.$route = $route;
    vm.$templateCache = $templateCache;
    vm.$utilityService = $utilityService;

    vm.onOfficeHoursFormSubmit = _onOfficeHoursFormSubmit;

    vm.openCalendar = _openCalendar;
    vm.settings = _settings;

    vm.getOfficeHourSessionForEdit = _getOfficeHourSessionForEdit;

    vm.onReceiveSectionList = _onReceiveSectionList;
    vm.onReceiveSectionListError = _onReceiveSectionListError;

    vm.onOfficeHourValidateForm = _onOfficeHourValidateForm;

    vm.onSubmitOfficeHoursFormSuccess = _onSubmitOfficeHoursFormSuccess;
    vm.onSubmitOfficeHoursFormError = _onSubmitOfficeHoursFormError;

    vm.resetOfficeHoursForm = _resetOfficeHoursForm;

    

    vm.onUpdateOfficeHoursFormSuccess = _onUpdateOfficeHoursFormSuccess;
    vm.onUpdateOfficeHoursFormError = _onUpdateOfficeHoursFormError;

    vm.submitOfficeHourFormData = _submitOfficeHourFormData;

    vm.onGetOfficeHourSessionSuccess = _onGetOfficeHourSessionSuccess;
    vm.onGetOfficeHourSessionError = _onGetOfficeHourSessionError;

    vm.convertTimeDateToDateObject = _convertTimeDateToDateObject;




    //vm.officeHour.Topic
    vm.showOfficeHourFormErrors = true;
    vm.minDate = new Date();
    vm.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };
    vm.minTime = new Date().getHours() + ":" + new Date().getMinutes() + ":" + new Date().getSeconds();
    vm.ckeditorOptions = {
        language: 'en',
        allowedContent: true,
        entities: false
    }
    vm.session = {};


    vm.notify = vm.$officeHoursService.getNotifier($scope);

    render();

    function render() {
        vm.settings();
        vm.$officeHoursService.getSectionsList(vm.onReceiveSectionList, vm.onReceiveSectionListError);
        vm.getOfficeHourSessionForEdit();
     
    }

    function _getOfficeHourSessionForEdit() {
        if (vm.$routeParams.officeHoursId != 0 && vm.$routeParams.officeHoursId != null) {
            vm.session.id = vm.$routeParams.officeHoursId;
            vm.$officeHoursService.getOfficeHourSession(vm.session.id, vm.onGetOfficeHourSessionSuccess, vm.onGetOfficeHourSessionError);
            vm.addSchedule = false;
            vm.updateSchedule = true;

        }
    }

    function _onGetOfficeHourSessionSuccess(data) {
        //this receives the data and calls the special
        //notify method that will trigger ng to refresh UI
        vm.notify(function () {
            vm.session = data.item;
            vm.convertTimeDateToDateObject();
            console.log(vm.session);
        });
    }

    function _convertTimeDateToDateObject() {
        vm.session.startTime = vm.$utilityService.dateFromMilitaryTime(vm.session.startTime);
        vm.session.endTime = vm.$utilityService.dateFromMilitaryTime(vm.session.endTime);

        if (vm.session.date)
            vm.session.date = new Date(vm.session.date);
    }

    function _onGetOfficeHourSessionError(jqXhr, error) {
        console.log(error);
    }


    function _onReceiveSectionList(data) {
        //this receives the data and calls the special
        //notify method that will trigger ng to refresh UI
        vm.notify(function () {
            vm.courses = data.items;
            console.log(vm.courses);
        });
    }

    function _onReceiveSectionListError(jqXhr, error) {
        console.log(error);
    }

    function _onOfficeHoursFormSubmit() {
        vm.onOfficeHourValidateForm();
    }

    function _resetOfficeHoursForm($event) {
        $event.stopPropagation();
        console.log("reset!!!")
        var currentPageTemplate = vm.$route.current.templateUrl;
        vm.$templateCache.remove(currentPageTemplate);
        vm.$route.reload();
    }


    function _onOfficeHourValidateForm() {
        vm.showOfficeHourFormErrors = true;
       
        if (vm.officeHourForm.$valid) {
            vm.submitOfficeHourFormData();
        }
    };

    function _submitOfficeHourFormData(){
        vm.session.date = vm.$utilityService.serializeDatepicker(vm.session.date);
        vm.session.startTime = vm.$utilityService.militaryTimeFromDate(vm.session.startTime);
        vm.session.endTime = vm.$utilityService.militaryTimeFromDate(vm.session.endTime);
        vm.session.instructorId = 2;


        if (vm.$routeParams.officeHoursId != null || vm.session.id != null) {
            //vm.session.id = angular.element("#id").val();
            //vm.session.date = vm.session.date.toLocaleDateString("en-US");
            var formData = vm.session;
            vm.$officeHoursService.updateOfficeHoursForm(formData, vm.session.id, vm.onUpdateOfficeHoursFormSuccess, vm.onUpdateOfficeHoursFormError);
        }
        else {
            //vm.session.date = vm.session.date.toLocaleDateString("en-US");
            var formData = vm.session;
            vm.$officeHoursService.submitOfficeHoursForm(formData, vm.onSubmitOfficeHoursFormSuccess, vm.onSubmitOfficeHoursFormError);
        }
    }

 
    function _onUpdateOfficeHoursFormSuccess(data) {
        //this receives the data and calls the special
        //notify method that will trigger ng to refresh UI
        vm.notify(function () {
            vm.convertTimeDateToDateObject();
            vm.showOfficeHourFormErrors = false;
            console.log(data);

        });
    }

    function _onUpdateOfficeHoursFormError(jqXhr, error) {
        vm.convertTimeDateToDateObject();
        console.log(error);
    }



    function _onSubmitOfficeHoursFormSuccess(data) {
        //this receives the data and calls the special
        //notify method that will trigger ng to refresh UI
        vm.notify(function () {
            console.log(data);
            vm.session.id = data.item;
            vm.convertTimeDateToDateObject();
            vm.showOfficeHourFormErrors = false;
            vm.addSchedule = false;
            vm.updateSchedule = true;
        });
    }

    function _onSubmitOfficeHoursFormError(jqXhr, error) {
        vm.convertTimeDateToDateObject();
        console.log(error);
    }



    function _openCalendar($event) {
        vm.dateToday = new Date();
        vm.startStatus.opened = true;
    }

    function _settings() {
        vm.startStatus = {
            opened: false
        };
        vm.showOfficeHourFormErrors = false;
        vm.addSchedule = true;
        vm.updateSchedule = false;
        vm.timeZones = [{ id: 1, tZone: 'Atlantic Standard Time (AST)' }, { id: 2, tZone: 'Eastern Standard Time (EST)' }, { id: 3, tZone: 'Central Standard Time (CST)' }, { id: 4, tZone: 'Mountain Standard Time (MST)' }, { id: 5, tZone: 'Pacific Standard Time (PST)' }, { id: 6, tZone: 'Alaskan Standard Time (AKST)' }, { id: 7, tZone: 'Hawaii-Aleutian Standard Time (HST)' }, { id: 8, tZone: 'Samoa Standard Time (UTC-11)' }, { id: 9, tZone: 'Chamorro Standard Time (UTC+10)' }];

    }
}


sabio.ng.addController(sabio.ng.app.module
, "officeHoursManagerController"
, ['$scope', '$baseController', '$officeHoursService', '$routeParams', '$location', '$route', '$templateCache', '$utilityService']
, sabio.page.officeHoursManagerControllerFactory);


