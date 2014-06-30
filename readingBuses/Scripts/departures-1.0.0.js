// TODO namespace

function parseDate(date) {
    return new Date(parseInt(date.substr(6)));
}

function removeSeconds(time) {
    return time.substring(0, time.length - 3);
}

function Departure(departsUtc, service, busStop, destination, reachable) {
    this.DepartsUtc = departsUtc;
    this.Departs = removeSeconds(departsUtc.toLocaleTimeString());
    this.DepartsIn = ko.observable(0);
    this.Service = service;
    this.BusStop = busStop;
    this.Destination = destination;
    this.Reachable = reachable;
}

Departure.prototype.calculate = function (nowUtc) {
    var minutes = (this.DepartsUtc.getTime() - nowUtc.getTime()) / (60 * 1000);
    minutes = Math.floor(minutes);
    this.DepartsIn(minutes);
}

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

    self.addDeparture = function (nowUtc, departsUtc, service, busStop, destination, reachable) {
        var item = ko.observable(new Departure(parseDate(departsUtc), service, busStop, destination, reachable));
        item().calculate(nowUtc);
        self.departures.push(item);
    };

    self.timer = null;

    self.stopRefreshingTimes = function () {
        if (self.timer !== null) {
            window.clearInterval(self.timer);
            self.timer = null;
        }
    }

    self.refreshTimeToDeparture = function () {
        var now = new Date(),
            item,
            i, 
            catchable = 0;
        for (i = 0; i < self.departures().length; i++) {
            item = self.departures()[i]();
            item.calculate(now);
            if (item.DepartsIn() > 0) {
                catchable += 1;
            }
        }
        if (catchable === 0) {
            self.stopRefreshingTimes();
        }
    };

    self.getDepartures = function (route) {
        self.stopRefreshingTimes();

        self.departures = ko.observableArray();
        if (route) {
            var start = new Date().getTime();
            self = this;
            context.locker(true); //lock list
            $.getJSON(self.getDeparturesApiPath, { routeName: route }
                ).done(function (data) {
                    var nowUtc = parseDate(data.TimeStamp);
                    $.each(data.Departures, function (key, value) {
                        self.addDeparture(nowUtc, value.DepartsUtc, value.Service, value.BusStop, value.Destination, value.Reachable);
                    });
                    self.departureRoute(data.Route);
                }).done(function (data) {
                    self.timer = window.setInterval(self.refreshTimeToDeparture, 30 * 1000);
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