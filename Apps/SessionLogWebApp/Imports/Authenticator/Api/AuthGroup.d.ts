import { AppApiGroup } from "XtiShared/AppApiGroup";
import { AppApiView } from "XtiShared/AppApiView";
import { AppApiEvents } from "XtiShared/AppApiEvents";
import { AppResourceUrl } from "XtiShared/AppResourceUrl";
export declare class AuthGroup extends AppApiGroup {
    constructor(events: AppApiEvents, resourceUrl: AppResourceUrl);
    readonly Index: AppApiView<IEmptyRequest>;
    private readonly VerifyAction;
    readonly Login: AppApiView<ILoginModel>;
    readonly Logout: AppApiView<IEmptyRequest>;
    Verify(model: ILoginCredentials, errorOptions?: IActionErrorOptions): Promise<IEmptyActionResult>;
}
