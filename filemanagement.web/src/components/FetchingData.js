import React, { useState, useEffect } from "react";
import Loader from "react-loader-spinner";

import { getfolders } from "../services/folderService";
import FolderTable from "./FolderTable";
import "../styles/table.css";

const FetchingData = (props) => {
  const [folders, setFolders] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  useEffect(() => {
    setIsLoading(true);
    getfolders()
      .then((res) => {
        setFolders(res.data.data);
      })
      .catch((err) => {
        setError(err);
      });
    setIsLoading(false);
  }, []);

  if (error) {
    return (
      <p>
        Dosyaları çekerken bir hata oluştu. Lütfen tekrar giriş yapınız. {error}
      </p>
    );
  }

  if (isLoading) {
    return (
      <Loader
        type="Puff"
        color="#00BFFF"
        height={100}
        width={100}
        visible={isLoading}
      />
    );
  }

  return (
    <div className="table">
      {folders.length === 0 && !error && !isLoading ? (
        <p>Hiç klasör bulunamadı. Yeni klasör eklemek ister misiniz?</p>
      ) : (
        <FolderTable data={folders} />
      )}
    </div>
  );
};

export default FetchingData;
