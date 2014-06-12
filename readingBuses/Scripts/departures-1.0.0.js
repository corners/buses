// TODO namespace

function BusDepartureViewModel(getRoutesApiPath, getDeparturesApiPath) {
    // Data
    var self = this;
    context = self;
    self.locker = ko.observable(false);

    self.routes = ko.observableArray([]);
    self.selectedRoute = ko.observable();
    self.departures = ko.observableArray([]);
    self.getRoutesApiPath = getRoutesApiPath;
    self.getDeparturesApiPath = getDeparturesApiPath;

    self.addRoute = function (name) {
        self.routes.push(name);
    };

    self.getRoutes = function () {
        $.getJSON(self.getRoutesApiPath
            ).done(function (data) {
                $.each(data.Routes, function (key, value) {
                    self.addRoute(value);
                });
                self.selectedRoute("");
            }).fail(function () {
                console.log('fail');
            });
    };

    self.addDeparture = function (departsIn, service, busStop, destination, reachable) {
        self.departures.push({ DepartsIn: departsIn, Service: service, BusStop: busStop, Destination: destination, Reachable: reachable });
    };

    self.getDepartures = function (route) {
        self.departures = ko.observableArray();
        if (route) {
            var start = new Date().getTime();
            self = this;
            context.locker(true); //lock list
            $.getJSON(self.getDeparturesApiPath, { routeName: route }
                ).done(function (data) {
                    $.each(data.Departures, function (key, value) {
                        self.addDeparture(value.DepartsIn, value.Service, value.BusStop, value.Destination, value.Reachable);
                    });
                    self.departureRoute(data.Route);
                }).done(function (data) {
                    var time_ms = (new Date().getTime()) - start;
                    self.lastUpdated(new Date(parseInt(data.TimeStamp.substr(6))) + '. Data retrieved in ' + time_ms + 'ms');
                }).fail(function () {
                    console.log("fail");
                }).always(function () {
                    context.locker(false);  //unlock list
                });
        }
    };

    /// The route name for the displayed departures
    self.departureRoute = ko.observable('');
    /// The server time the departures were last updated
    self.lastUpdated = ko.observable('');

    self.refreshDepartures = function () {
        self.getDepartures(self.selectedRoute());
    };

    /// Required to trigger getting the departure list
    self.itemsTrigger = ko.computed(function () {
        return self.getDepartures(self.selectedRoute());
    }, this);
}