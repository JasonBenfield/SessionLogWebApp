// Generated code

import { AppApiGroup } from "../../Shared/AppApiGroup";
import { AppApiAction } from "../../Shared/AppApiAction";
import { AppApiView } from "../../Shared/AppApiView";
import { AppApiEvents } from "../../Shared/AppApiEvents";
import { AppResourceUrl } from "../../Shared/AppResourceUrl";

export class PermanentLogGroup extends AppApiGroup {
	constructor(events: AppApiEvents, resourceUrl: AppResourceUrl) {
		super(events, resourceUrl, 'PermanentLog');
		this.LogBatchAction = this.createAction<ILogBatchModel,IEmptyActionResult>('LogBatch', 'LogBatch');
		this.StartSessionAction = this.createAction<IStartSessionModel,IEmptyActionResult>('StartSession', 'StartSession');
		this.StartRequestAction = this.createAction<IStartRequestModel,IEmptyActionResult>('StartRequest', 'StartRequest');
		this.EndRequestAction = this.createAction<IEndRequestModel,IEmptyActionResult>('EndRequest', 'EndRequest');
		this.EndSessionAction = this.createAction<IEndSessionModel,IEmptyActionResult>('EndSession', 'EndSession');
		this.LogEventAction = this.createAction<ILogEventModel,IEmptyActionResult>('LogEvent', 'LogEvent');
		this.AuthenticateSessionAction = this.createAction<IAuthenticateSessionModel,IEmptyActionResult>('AuthenticateSession', 'AuthenticateSession');
		this.EndExpiredSessionsAction = this.createAction<IEmptyRequest,IEmptyActionResult>('EndExpiredSessions', 'EndExpiredSessions');
	}

	private readonly LogBatchAction: AppApiAction<ILogBatchModel,IEmptyActionResult>;
	private readonly StartSessionAction: AppApiAction<IStartSessionModel,IEmptyActionResult>;
	private readonly StartRequestAction: AppApiAction<IStartRequestModel,IEmptyActionResult>;
	private readonly EndRequestAction: AppApiAction<IEndRequestModel,IEmptyActionResult>;
	private readonly EndSessionAction: AppApiAction<IEndSessionModel,IEmptyActionResult>;
	private readonly LogEventAction: AppApiAction<ILogEventModel,IEmptyActionResult>;
	private readonly AuthenticateSessionAction: AppApiAction<IAuthenticateSessionModel,IEmptyActionResult>;
	private readonly EndExpiredSessionsAction: AppApiAction<IEmptyRequest,IEmptyActionResult>;

	LogBatch(model: ILogBatchModel, errorOptions?: IActionErrorOptions) {
		return this.LogBatchAction.execute(model, errorOptions || {});
	}
	StartSession(model: IStartSessionModel, errorOptions?: IActionErrorOptions) {
		return this.StartSessionAction.execute(model, errorOptions || {});
	}
	StartRequest(model: IStartRequestModel, errorOptions?: IActionErrorOptions) {
		return this.StartRequestAction.execute(model, errorOptions || {});
	}
	EndRequest(model: IEndRequestModel, errorOptions?: IActionErrorOptions) {
		return this.EndRequestAction.execute(model, errorOptions || {});
	}
	EndSession(model: IEndSessionModel, errorOptions?: IActionErrorOptions) {
		return this.EndSessionAction.execute(model, errorOptions || {});
	}
	LogEvent(model: ILogEventModel, errorOptions?: IActionErrorOptions) {
		return this.LogEventAction.execute(model, errorOptions || {});
	}
	AuthenticateSession(model: IAuthenticateSessionModel, errorOptions?: IActionErrorOptions) {
		return this.AuthenticateSessionAction.execute(model, errorOptions || {});
	}
	EndExpiredSessions(errorOptions?: IActionErrorOptions) {
		return this.EndExpiredSessionsAction.execute({}, errorOptions || {});
	}
}