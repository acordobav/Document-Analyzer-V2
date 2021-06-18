// import axios from 'axios';
// import history from '../../services/history';
// import { AUTHAPI_URL } from '../../helpers/constants';

// export class AuthService {
//   static token: string;
  
//   static storeToken(token: string) {
//     this.token = token;
//     sessionStorage.setItem(btoa('token'), btoa(token));
//   }

//   static autoLogin() {
//     let storedToken = sessionStorage.getItem(btoa('token'));
//     if(!storedToken) return;

//     // The token is stored
//     this.token = atob(storedToken);
    
//     // Checks if the user is on the login page or in the default page
//     const currentPath = history.location.pathname;
//     if(!(currentPath === '/login' || currentPath === '/')) return; 
    
//     // Redirection to the files page
//     history.push({
//       pathname: '/files',
//       state: { detail: this.state }
//     })
//   }

//   static logout() {
//     sessionStorage.clear();
//   }

//   static loginRequest(email: string, password: string) {
//     return axios.post(AUTHAPI_URL + 'Login', {
//       email: email,
//       password: btoa(password), // The password is encrypted in base 64
//     });
//   }
// }

// export default AuthService;

export default function name(test:string) {
  console.log(test);
}