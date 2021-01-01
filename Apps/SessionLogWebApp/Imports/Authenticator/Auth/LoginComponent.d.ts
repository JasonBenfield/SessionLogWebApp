import { LoginComponentViewModel } from './LoginComponentViewModel';
import { AuthenticatorAppApi } from '../Api/AuthenticatorAppApi';
export declare class LoginResult {
    readonly token: string;
    constructor(token: string);
}
export declare class LoginComponent {
    private readonly vm;
    private readonly authenticator;
    constructor(vm: LoginComponentViewModel, authenticator: AuthenticatorAppApi);
    private readonly awaitable;
    private readonly alert;
    private readonly userName;
    private readonly password;
    private readonly loginCommand;
    start(): Promise<LoginResult>;
    private login;
    private getCredentials;
    private verifyLogin;
    private postLogin;
    private createInput;
}
