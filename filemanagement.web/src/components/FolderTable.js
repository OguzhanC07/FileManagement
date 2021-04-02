import React, { useContext, useState } from "react";
import uuid from "uuid/dist/v1";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useTranslation } from "react-i18next";

import { FolderContext } from "../context/FolderContext";

import { downloadfolder } from "../services/folderService";
import { getsinglefile } from "../services/fileService";
import { FileContext } from "../context/FileContext";
import "../styles/react-contextmenu.css";
import TableWrapper from "./TableWrapper";
import ContextMenuCreator from "./ContextMenuCreator";
import TableFolderRow from "./TableFolderRow";
import TableFileRow from "./TableFileRow";

const FolderTable = (props) => {
  const [isDisabled, setIsDisabled] = useState(false);

  const { folder } = useContext(FolderContext);
  const { file } = useContext(FileContext);
  const { t } = useTranslation();

  function collect(props) {
    return { id: props.folderId, name: props.name, type: props.type };
  }

  const MENU_TYPE = "SIMPLE";
  const FILE_MENU = "FILE_MENU";

  const downloadHandler = async (id, type) => {
    let response = "";
    let problem = "";
    switch (type) {
      case "file":
        try {
          setIsDisabled(true);
          response = await getsinglefile(id);
          setIsDisabled(false);
        } catch (responseError) {
          problem = responseError;
          setIsDisabled(false);
        }
        break;
      case "folder":
        try {
          setIsDisabled(true);
          response = await downloadfolder(id);
          setIsDisabled(false);
        } catch (responseError) {
          problem = responseError;
          setIsDisabled(false);
        }
        break;
      default:
        break;
    }
    if (typeof problem === "undefined" || problem !== "") {
      toast.error(
        t("folderTable.downloadError") + t("responseErrors.wentWrong")
      );
    } else {
      setIsDisabled(true);
      var data = new Blob([response.data], { type: response.data.type });
      var folderData = window.URL.createObjectURL(data);
      const tempLink = document.createElement("a");
      tempLink.href = folderData;
      tempLink.setAttribute("download", uuid());
      tempLink.click();
      setIsDisabled(false);
    }
  };

  const convertHandler = (bytes) => {
    var sizes = [t("folderTable.bytes"), "KB", "MB", "GB", "TB"];
    if (bytes === 0) return "0 Byte";
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + " " + sizes[i];
  };

  const handleDownloadClick = (e, data) => {
    downloadHandler(data.id, data.type);
  };

  const handleDoubleClick = (id, data) => {
    if (!isNaN(id)) {
      props.openFolder(id, data);
    } else {
      props.openFolder(data.id, data.name);
    }
  };

  return (
    <div>
      <TableWrapper>
        {folder.folders.map((folder) => (
          <TableFolderRow
            convertHandler={convertHandler}
            handleDoubleClick={handleDoubleClick}
            collect={collect}
            folder={folder}
            isDisabled={isDisabled}
            type={MENU_TYPE}
            key={folder.id}
          />
        ))}
        {file.files.length > 0
          ? file.files.map((file) => (
              <TableFileRow
                file={file}
                collect={collect}
                convertHandler={convertHandler}
                isDisabled={isDisabled}
                type={FILE_MENU}
                key={file.id}
              />
            ))
          : null}
      </TableWrapper>

      <ToastContainer
        position="top-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />

      <ContextMenuCreator
        handleDownload={handleDownloadClick}
        type={FILE_MENU}
      />
      <ContextMenuCreator
        handleOpen={handleDoubleClick}
        handleDownload={handleDownloadClick}
        type={MENU_TYPE}
      />
    </div>
  );
};

export default FolderTable;
