sabio.ng.app.module.config(function ($routeProvider, $locationProvider) {

    $routeProvider.when('/', {
        templateUrl: '/Scripts/app/admin/officeHours/templates/index.html',
        controller: 'officeHoursListController',
        controllerAs: 'officeHour',
        reloadOnSearch: false
    })
    .when('/add', {
        templateUrl: '/Scripts/app/admin/officeHours/templates/manage.html',
        controller: 'officeHoursManagerController',
        controllerAs: 'officeHour',
        reloadOnSearch: false
    })
    .when('/edit/:officeHoursId', {
        templateUrl: '/Scripts/app/admin/officeHours/templates/manage.html',
        controller: 'officeHoursManagerController',
        controllerAs: 'officeHour',
        reloadOnSearch: false
    })
    .otherwise({
        redirectTo: '/'
    });

    $locationProvider.html5Mode({
        enabled: false
    });

});