"use strict";
// Generated code
Object.defineProperty(exports, "__esModule", { value: true });
exports.SessionLogAppApi = void 0;
var tslib_1 = require("tslib");
var AppApi_1 = require("../../Shared/AppApi");
var PermanentLogGroup_1 = require("./PermanentLogGroup");
var SessionLogAppApi = /** @class */ (function (_super) {
    tslib_1.__extends(SessionLogAppApi, _super);
    function SessionLogAppApi(events, baseUrl, version) {
        if (version === void 0) { version = ''; }
        var _this = _super.call(this, events, baseUrl, 'SessionLog', version || SessionLogAppApi.DefaultVersion) || this;
        _this.PermanentLog = _this.addGroup(function (evts, resourceUrl) { return new PermanentLogGroup_1.PermanentLogGroup(evts, resourceUrl); });
        return _this;
    }
    SessionLogAppApi.DefaultVersion = 'V1124';
    return SessionLogAppApi;
}(AppApi_1.AppApi));
exports.SessionLogAppApi = SessionLogAppApi;
//# sourceMappingURL=SessionLogAppApi.js.map