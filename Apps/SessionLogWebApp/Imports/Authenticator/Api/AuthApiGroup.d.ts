import { AppApiGroup } from "XtiShared/AppApiGroup";
import { AppApiEvents } from "XtiShared/AppApiEvents";
import { AppResourceUrl } from "XtiShared/AppResourceUrl";
export declare class AuthApiGroup extends AppApiGroup {
    constructor(events: AppApiEvents, resourceUrl: AppResourceUrl);
    private readonly AuthenticateAction;
    Authenticate(model: ILoginCredentials, errorOptions?: IActionErrorOptions): Promise<ILoginResult>;
}
