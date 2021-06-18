import * as React from 'react';
import { Link } from 'react-router-dom';

type MyProps = {
    message: string;
  };

type MyState = {
    count: number; 
};

export default  class One extends React.Component<MyProps, MyState> {
    state: MyState = {
      count: 0,
    };
    
    render() {
      return (
        <div>
          {this.props.message} {this.state.count}
          <Link to='/files'>Goto Page Two</Link>
        </div>
      );
    }
  }