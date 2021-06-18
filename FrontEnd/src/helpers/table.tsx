import * as React from 'react';
import { DataGrid } from "@material-ui/data-grid";

 type MyProps = {
    rows: any;
    columns: any;
    pageSize: number;
    height: number;
};

export default function DataTable(props: MyProps) {

    return (
    <div style={{ height: props.height, width: '100%' }}>
        <DataGrid rows={props.rows} columns={props.columns} pageSize={props.pageSize} />
        {/* <DataGrid  rows={data} columns={columns} pageSize={5} checkboxSelection /> */}
    </div>
    );
}

// const this_json = {
//     columns: [{field: "product", headerName: "Product", width: 110}, {field: "qty", headerName: "Quantity", width: 110}],
//     rows: [{id: "0", product: "Papas", qty: 500}, {id: "1", product: "Tomate", qty: 100}]
// };
