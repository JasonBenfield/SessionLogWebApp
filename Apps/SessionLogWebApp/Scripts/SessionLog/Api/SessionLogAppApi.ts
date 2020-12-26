// Generated code

import { AppApi } from "../../Shared/AppApi";
import { AppApiEvents } from "../../Shared/AppApiEvents";
import { UserGroup } from "./UserGroup";
import { PermanentLogGroup } from "./PermanentLogGroup";

export class SessionLogAppApi extends AppApi {
	public static readonly DefaultVersion = 'V1124';

	constructor(events: AppApiEvents, baseUrl: string, version: string = '') {
		super(events, baseUrl, 'SessionLog', version || SessionLogAppApi.DefaultVersion);
		this.User = this.addGroup((evts, resourceUrl) => new UserGroup(evts, resourceUrl));
		this.PermanentLog = this.addGroup((evts, resourceUrl) => new PermanentLogGroup(evts, resourceUrl));
	}

	readonly User: UserGroup;
	readonly PermanentLog: PermanentLogGroup;
}