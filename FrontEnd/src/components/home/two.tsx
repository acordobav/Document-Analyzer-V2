import * as React from 'react';
import { Link } from 'react-router-dom';

export default class Two extends React.Component<any, any> {
    render() {
      return (
        <div>
          <div>TWO</div>
          <Link to='/'>Goto Home</Link>
        </div>
      );
    }
  }