import * as React from 'react';
import * as ReactDOM from 'react-dom';
import './index.scss';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { AuthService, RenderFunction } from './auth/AuthService';
// import axios from 'axios';

const renderApp: RenderFunction = () => ReactDOM.render(  
  <React.StrictMode>
    <App></App>
  </React.StrictMode>
  ,
  document.getElementById('root')
);

const auth = AuthService.getInstance();
auth.initKeycloak(renderApp);


// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

/*
axios.interceptors.request.use((config) => {
  const token = AuthService.token;

  // Checks if the token is invalid
  if(!token) return config;

  config.headers.Authorization = 'Bearer ' + token;
  return config;
});

*/
