import React from "react";
import { Icon } from "semantic-ui-react";
import { ContextMenu, MenuItem } from "react-contextmenu";
import { useTranslation } from "react-i18next";

const ContextMenuCreator = ({ handleOpen, handleDownload, type }) => {
  const { t } = useTranslation();

  return (
    <ContextMenu id={type}>
      <MenuItem onClick={handleDownload} data={{ item: "download" }}>
        <Icon name="download" /> {t("contextMenu.download")}
      </MenuItem>
      {type === "SIMPLE" && <MenuItem divider />}
      {type === "SIMPLE" && (
        <MenuItem onClick={handleOpen} data={{ item: "open" }}>
          <Icon name="folder open" /> {t("contextMenu.openFolder")}
        </MenuItem>
      )}
    </ContextMenu>
  );
};

export default ContextMenuCreator;
