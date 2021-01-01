"use strict";
// Generated code
Object.defineProperty(exports, "__esModule", { value: true });
exports.PermanentLogGroup = void 0;
var tslib_1 = require("tslib");
var AppApiGroup_1 = require("XtiShared/AppApiGroup");
var PermanentLogGroup = /** @class */ (function (_super) {
    tslib_1.__extends(PermanentLogGroup, _super);
    function PermanentLogGroup(events, resourceUrl) {
        var _this = _super.call(this, events, resourceUrl, 'PermanentLog') || this;
        _this.LogBatchAction = _this.createAction('LogBatch', 'LogBatch');
        _this.StartSessionAction = _this.createAction('StartSession', 'StartSession');
        _this.StartRequestAction = _this.createAction('StartRequest', 'StartRequest');
        _this.EndRequestAction = _this.createAction('EndRequest', 'EndRequest');
        _this.EndSessionAction = _this.createAction('EndSession', 'EndSession');
        _this.LogEventAction = _this.createAction('LogEvent', 'LogEvent');
        _this.AuthenticateSessionAction = _this.createAction('AuthenticateSession', 'AuthenticateSession');
        _this.EndExpiredSessionsAction = _this.createAction('EndExpiredSessions', 'EndExpiredSessions');
        return _this;
    }
    PermanentLogGroup.prototype.LogBatch = function (model, errorOptions) {
        return this.LogBatchAction.execute(model, errorOptions || {});
    };
    PermanentLogGroup.prototype.StartSession = function (model, errorOptions) {
        return this.StartSessionAction.execute(model, errorOptions || {});
    };
    PermanentLogGroup.prototype.StartRequest = function (model, errorOptions) {
        return this.StartRequestAction.execute(model, errorOptions || {});
    };
    PermanentLogGroup.prototype.EndRequest = function (model, errorOptions) {
        return this.EndRequestAction.execute(model, errorOptions || {});
    };
    PermanentLogGroup.prototype.EndSession = function (model, errorOptions) {
        return this.EndSessionAction.execute(model, errorOptions || {});
    };
    PermanentLogGroup.prototype.LogEvent = function (model, errorOptions) {
        return this.LogEventAction.execute(model, errorOptions || {});
    };
    PermanentLogGroup.prototype.AuthenticateSession = function (model, errorOptions) {
        return this.AuthenticateSessionAction.execute(model, errorOptions || {});
    };
    PermanentLogGroup.prototype.EndExpiredSessions = function (errorOptions) {
        return this.EndExpiredSessionsAction.execute({}, errorOptions || {});
    };
    return PermanentLogGroup;
}(AppApiGroup_1.AppApiGroup));
exports.PermanentLogGroup = PermanentLogGroup;
//# sourceMappingURL=PermanentLogGroup.js.map