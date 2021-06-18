declare global {
    interface Window {
        _env_: any;
    }
  }

const apiHost = window._env_.DOCANALYZER_HOST;
const apiPort = window._env_.DOCANALYZER_PORT;
const wsHost = window._env_.WEBSOCKET_HOST;
const wsPort = window._env_.WEBSOCKET_PORT;


export const urlAPI = 'http://' + apiHost + ':' + apiPort + '/';
export const WEB_SOCKET = 'ws://' + wsHost + ':' + wsPort;
//export const urlAPI = 'http://localhost:39748/';
