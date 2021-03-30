import React from "react";
import { Icon } from "semantic-ui-react";

const SortingArrow = ({ sortType }) => {
  return sortType === "asc" ? (
    <Icon name="angle down" />
  ) : (
    <Icon name="angle up" />
  );
};

export default SortingArrow;
