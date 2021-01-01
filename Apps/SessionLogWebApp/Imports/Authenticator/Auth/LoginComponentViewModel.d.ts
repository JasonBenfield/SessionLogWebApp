import { TextInputViewModel } from "XtiShared/TextInput";
import { OffscreenSubmitViewModel } from 'XtiShared/OffscreenSubmitViewModel';
import * as ko from 'knockout';
import { AlertViewModel } from 'XtiShared/Alert';
export declare class LoginComponentViewModel {
    constructor();
    readonly componentName: ko.Observable<string>;
    readonly alert: AlertViewModel;
    readonly userName: TextInputViewModel;
    readonly password: TextInputViewModel;
    readonly loginCommand: import("../../../Imports/Shared/Command").CommandViewModel;
    readonly offscreenSubmit: OffscreenSubmitViewModel;
}
