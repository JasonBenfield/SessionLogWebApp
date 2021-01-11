import { LoginComponentViewModel } from './LoginComponentViewModel';
import { AuthenticatorAppApi } from '../Api/AuthenticatorAppApi';
export declare class LoginResult {
    readonly token: string;
    constructor(token: string);
}
export declare class LoginComponent {
    private readonly vm;
    private readonly authApi;
    constructor(vm: LoginComponentViewModel, authApi: AuthenticatorAppApi);
    private readonly awaitable;
    private readonly alert;
    private readonly verifyLoginForm;
    private readonly loginCommand;
    start(): Promise<LoginResult>;
    private login;
    private getCredentials;
    private postLogin;
    private createInput;
}
