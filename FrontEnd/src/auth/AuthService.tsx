import Keycloak from "keycloak-js";


export type RenderFunction = {
    () : void;
}


/**
 * The AuthService class defines the `getInstance` method that lets clients access
 * the unique singleton instance.
 */
export class AuthService {
    private static instance: AuthService;

    private keycloak: any;

    private constructor() { }

    initKeycloak(onAuthenticatedCallback: RenderFunction) {
        this.keycloak = new (Keycloak as any)("./keycloak.json");

        this.keycloak.init({ onLoad: "login-required" }).then((authenticated: boolean) => {
            if (!authenticated) {
                window.location.reload();
            } else {
                onAuthenticatedCallback();
            }
        });
    }

    public static getInstance(): AuthService {
        if(!AuthService.instance) {
            AuthService.instance = new AuthService();
        }

        return AuthService.instance;
    }

    logout() {
        this.keycloak.logout();
    }

    isLoggedIn(): boolean {
        return !!this.keycloak.token;
    }

    getToken() {
        return this.keycloak.token;
    }

}
