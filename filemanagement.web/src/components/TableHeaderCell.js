import React from "react";
import { Table } from "semantic-ui-react";
import { useTranslation } from "react-i18next";

import SortingArrow from "./SortingArrow";

const TableHeaderCell = ({
  translateName,
  name,
  sortName,
  sortType,
  sortingHandler,
}) => {
  const { t } = useTranslation();
  return (
    <Table.HeaderCell
      style={{ cursor: "pointer" }}
      className="tableHeader"
      onClick={() => {
        sortingHandler(name);
      }}
    >
      {t(`folderTable.${translateName}`)}
      {sortName === name && <SortingArrow sortType={sortType} />}
    </Table.HeaderCell>
  );
};

export default TableHeaderCell;
