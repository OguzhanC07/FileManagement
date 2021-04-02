import React, { useState, useContext } from "react";
import { Table, TableHeader } from "semantic-ui-react";
import { useTranslation } from "react-i18next";

import TableHeaderCell from "./TableHeaderCell";
import { FolderContext, SORTFOLDERS } from "../context/FolderContext";
import { FileContext, SORTFILE } from "../context/FileContext";

const TableWrapper = (props) => {
  const [sortName, setSortName] = useState("name");
  const [sortType, setSortType] = useState("asc");
  const { dispatch } = useContext(FolderContext);
  const { dispatch: fileDispatch } = useContext(FileContext);
  const { t } = useTranslation();

  const sortingHandler = (type) => {
    sortType === "asc" ? setSortType("desc") : setSortType("asc");
    setSortName(type);
    switch (type) {
      case "name":
        dispatch({
          type: SORTFOLDERS,
          sortType: sortType,
          sortName: "folderName",
        });
        fileDispatch({
          type: SORTFILE,
          sortType: sortType,
          sortName: "fileName",
        });
        break;
      case "size":
        dispatch({
          type: SORTFOLDERS,
          sortType: sortType,
          sortName: "size",
        });
        fileDispatch({
          type: SORTFILE,
          sortType: sortType,
          sortName: "size",
        });
        break;
      case "time":
        dispatch({
          type: SORTFOLDERS,
          sortType: sortType,
          sortName: "createdAt",
        });
        fileDispatch({
          type: SORTFILE,
          sortType: sortType,
          sortName: "uploadedAt",
        });
        break;
      default:
        break;
    }
  };

  return (
    <Table style={{ paddingTop: 10 }} selectable>
      <TableHeader>
        <Table.Row>
          <TableHeaderCell
            translateName="tableHeaderName"
            name="name"
            sortName={sortName}
            sortType={sortType}
            sortingHandler={(type) => {
              sortingHandler(type);
            }}
          />
          <TableHeaderCell
            translateName="tableHeaderSize"
            name="size"
            sortName={sortName}
            sortType={sortType}
            sortingHandler={(type) => {
              sortingHandler(type);
            }}
          />
          <TableHeaderCell
            translateName="tableHeaderCreatedAt"
            name="time"
            sortName={sortName}
            sortType={sortType}
            sortingHandler={(type) => {
              sortingHandler(type);
            }}
          />
          <Table.HeaderCell className="tableHeader">
            {t("folderTable.tableHeaderOperations")}
          </Table.HeaderCell>
        </Table.Row>
      </TableHeader>
      <Table.Body>{props.children}</Table.Body>
    </Table>
  );
};

export default TableWrapper;
