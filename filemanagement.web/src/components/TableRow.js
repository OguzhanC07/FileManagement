import React from "react";
import { Table } from "semantic-ui-react";

const TableRow = (props) => {
  return (
    <Table.Row key={props.id}>
      <Table.Cell>{props.name}</Table.Cell>
      <Table.Cell>{props.size}</Table.Cell>
      <Table.Cell>{props.created}</Table.Cell>
    </Table.Row>
  );
};

export default TableRow;
