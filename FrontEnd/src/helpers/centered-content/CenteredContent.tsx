import React from "react";
import { Col, Row } from "react-bootstrap";
import "./CenteredContent.css";

type MyProps = {
    numRow: string;
    numCol: number;
};

class CenteredContent extends React.Component<MyProps> {
  render() {
    return (
      <>
        <Row className={this.props.numRow}>
          <Col sm={this.props.numCol} className="card text-white p-4">
            {this.props.children}
          </Col>
        </Row>
      </>
    );
  }
}

export default CenteredContent;
