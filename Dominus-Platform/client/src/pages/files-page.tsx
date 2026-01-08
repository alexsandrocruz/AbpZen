import { useState, useMemo, useRef } from "react";
import { useAuth } from "@/lib/auth-store";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api";
import { AppShell } from "@/components/layout/shell";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import { useToast } from "@/hooks/use-toast";
import { DeleteModal } from "@/components/modals/delete-modal";
import { ObjectUploader } from "@/components/ObjectUploader";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  Upload,
  MoreHorizontal,
  Trash2,
  Download,
  Search,
  FileText,
  FileImage,
  FileVideo,
  FileAudio,
  FileArchive,
  FileSpreadsheet,
  File as FileIcon,
  FolderOpen,
} from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import type { UppyFile, UploadResult } from "@uppy/core";

type FileRecord = {
  id: string;
  workspaceId: string;
  uploadedBy: string;
  fileName: string;
  originalName: string;
  fileUrl: string;
  fileSize: number;
  mimeType: string | null;
  folder: string | null;
  projectId: string | null;
  clientId: string | null;
  taskId: string | null;
  isPublic: boolean;
  createdAt: string;
};

type Project = {
  id: string;
  title: string;
};

type Client = {
  id: string;
  name: string;
};

function formatFileSize(bytes: number): string {
  if (bytes === 0) return "0 B";
  const k = 1024;
  const sizes = ["B", "KB", "MB", "GB"];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return parseFloat((bytes / Math.pow(k, i)).toFixed(1)) + " " + sizes[i];
}

function getFileIcon(mimeType: string | null) {
  if (!mimeType) return <FileIcon className="size-5 text-muted-foreground" />;

  if (mimeType.startsWith("image/")) {
    return <FileImage className="size-5 text-blue-500" />;
  }
  if (mimeType.startsWith("video/")) {
    return <FileVideo className="size-5 text-purple-500" />;
  }
  if (mimeType.startsWith("audio/")) {
    return <FileAudio className="size-5 text-pink-500" />;
  }
  if (mimeType.includes("pdf")) {
    return <FileText className="size-5 text-red-500" />;
  }
  if (mimeType.includes("zip") || mimeType.includes("rar") || mimeType.includes("tar") || mimeType.includes("gzip")) {
    return <FileArchive className="size-5 text-yellow-600" />;
  }
  if (mimeType.includes("spreadsheet") || mimeType.includes("excel") || mimeType.includes("csv")) {
    return <FileSpreadsheet className="size-5 text-green-500" />;
  }
  if (mimeType.includes("document") || mimeType.includes("word") || mimeType.includes("text")) {
    return <FileText className="size-5 text-blue-600" />;
  }

  return <FileIcon className="size-5 text-muted-foreground" />;
}

export default function FilesPage() {
  const { currentWorkspace } = useAuth();
  const { toast } = useToast();
  const queryClient = useQueryClient();

  const [searchQuery, setSearchQuery] = useState("");
  const [filterType, setFilterType] = useState<"all" | "project" | "client">("all");
  const [selectedProject, setSelectedProject] = useState<string | null>(null);
  const [selectedClient, setSelectedClient] = useState<string | null>(null);
  const [deleteFile, setDeleteFile] = useState<FileRecord | null>(null);
  const pendingUploadRef = useRef<{
    objectPath: string;
    fileName: string;
    fileSize: number;
    mimeType: string;
  } | null>(null);

  const { data: files = [], isLoading: filesLoading } = useQuery<FileRecord[]>({
    queryKey: ["files", currentWorkspace?.id, filterType === "project" ? selectedProject : null, filterType === "client" ? selectedClient : null],
    queryFn: () => {
      const filters: { projectId?: string; clientId?: string } = {};
      if (filterType === "project" && selectedProject) {
        filters.projectId = selectedProject;
      }
      if (filterType === "client" && selectedClient) {
        filters.clientId = selectedClient;
      }
      return api.getFiles(currentWorkspace!.id, filters);
    },
    enabled: !!currentWorkspace,
  });

  const { data: projects = [] } = useQuery<Project[]>({
    queryKey: ["projects", currentWorkspace?.id],
    queryFn: () => api.getProjects(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const { data: clients = [] } = useQuery<Client[]>({
    queryKey: ["clients", currentWorkspace?.id],
    queryFn: () => api.getClients(currentWorkspace!.id),
    enabled: !!currentWorkspace,
  });

  const createFileMutation = useMutation({
    mutationFn: (data: Parameters<typeof api.createFile>[1]) => api.createFile(currentWorkspace!.id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["files"] });
      toast({ title: "Sucesso", description: "Arquivo enviado com sucesso!" });
      pendingUploadRef.current = null;
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const deleteFileMutation = useMutation({
    mutationFn: (fileId: string) => api.deleteFile(currentWorkspace!.id, fileId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["files"] });
      toast({ title: "Sucesso", description: "Arquivo excluído com sucesso!" });
      setDeleteFile(null);
    },
    onError: (error: any) => {
      toast({ title: "Erro", description: error.message, variant: "destructive" });
    },
  });

  const filteredFiles = useMemo(() => {
    if (!searchQuery.trim()) return files;
    const query = searchQuery.toLowerCase();
    return files.filter(
      (file) =>
        file.originalName.toLowerCase().includes(query) ||
        file.fileName.toLowerCase().includes(query)
    );
  }, [files, searchQuery]);

  const handleGetUploadParameters = async (
    file: UppyFile<Record<string, unknown>, Record<string, unknown>>
  ): Promise<{ method: "PUT"; url: string; headers?: Record<string, string> }> => {
    const response = await fetch(`/api/workspaces/${currentWorkspace!.id}/files/request-url`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify({
        name: file.name,
        size: file.size,
        contentType: file.type || "application/octet-stream",
      }),
    });

    if (!response.ok) {
      throw new Error("Falha ao obter URL de upload");
    }

    const data = await response.json();
    
    pendingUploadRef.current = {
      objectPath: data.objectPath || data.uploadUrl,
      fileName: file.name,
      fileSize: file.size || 0,
      mimeType: file.type || "application/octet-stream",
    };

    return {
      method: "PUT",
      url: data.uploadUrl,
      headers: { "Content-Type": file.type || "application/octet-stream" },
    };
  };

  const handleUploadComplete = (result: UploadResult<Record<string, unknown>, Record<string, unknown>>) => {
    const pendingUpload = pendingUploadRef.current;
    if (result.successful && result.successful.length > 0 && pendingUpload) {
      createFileMutation.mutate({
        fileName: pendingUpload.fileName,
        originalName: pendingUpload.fileName,
        fileUrl: pendingUpload.objectPath,
        fileSize: pendingUpload.fileSize,
        mimeType: pendingUpload.mimeType,
        projectId: filterType === "project" && selectedProject ? selectedProject : undefined,
        clientId: filterType === "client" && selectedClient ? selectedClient : undefined,
      });
    }
  };

  const handleDownload = (file: FileRecord) => {
    const link = document.createElement("a");
    link.href = file.fileUrl;
    link.download = file.originalName;
    link.target = "_blank";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  const getProjectName = (projectId: string | null) => {
    if (!projectId) return null;
    const project = projects.find((p) => p.id === projectId);
    return project?.title || null;
  };

  const getClientName = (clientId: string | null) => {
    if (!clientId) return null;
    const client = clients.find((c) => c.id === clientId);
    return client?.name || null;
  };

  return (
    <AppShell>
      <div className="space-y-6">
        <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h1 className="text-3xl font-display font-bold tracking-tight" data-testid="page-title-files">
              Arquivos
            </h1>
            <p className="text-muted-foreground mt-1">
              Gerencie os arquivos do workspace
            </p>
          </div>
          <ObjectUploader
            maxNumberOfFiles={5}
            maxFileSize={52428800}
            onGetUploadParameters={handleGetUploadParameters}
            onComplete={handleUploadComplete}
          >
            <Upload className="size-4 mr-2" />
            Upload
          </ObjectUploader>
        </div>

        <div className="flex flex-col sm:flex-row gap-4">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-muted-foreground" />
            <Input
              placeholder="Buscar arquivos..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="pl-9"
              data-testid="input-search-files"
            />
          </div>

          <div className="flex gap-2">
            <Select
              value={filterType}
              onValueChange={(value: "all" | "project" | "client") => {
                setFilterType(value);
                setSelectedProject(null);
                setSelectedClient(null);
              }}
            >
              <SelectTrigger className="w-[160px]" data-testid="select-filter-type">
                <SelectValue placeholder="Filtrar por" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">Todos</SelectItem>
                <SelectItem value="project">Por Projeto</SelectItem>
                <SelectItem value="client">Por Cliente</SelectItem>
              </SelectContent>
            </Select>

            {filterType === "project" && (
              <Select
                value={selectedProject || ""}
                onValueChange={(value) => setSelectedProject(value || null)}
              >
                <SelectTrigger className="w-[200px]" data-testid="select-project">
                  <SelectValue placeholder="Selecione projeto" />
                </SelectTrigger>
                <SelectContent>
                  {projects.map((project) => (
                    <SelectItem key={project.id} value={project.id}>
                      {project.title}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            )}

            {filterType === "client" && (
              <Select
                value={selectedClient || ""}
                onValueChange={(value) => setSelectedClient(value || null)}
              >
                <SelectTrigger className="w-[200px]" data-testid="select-client">
                  <SelectValue placeholder="Selecione cliente" />
                </SelectTrigger>
                <SelectContent>
                  {clients.map((client) => (
                    <SelectItem key={client.id} value={client.id}>
                      {client.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            )}
          </div>
        </div>

        {filesLoading ? (
          <div className="space-y-3">
            {[...Array(5)].map((_, i) => (
              <Skeleton key={i} className="h-16 w-full rounded-lg" />
            ))}
          </div>
        ) : filteredFiles.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-16 text-center border border-dashed rounded-lg">
            <FolderOpen className="size-12 text-muted-foreground mb-4" />
            <h3 className="text-lg font-semibold mb-2">Nenhum arquivo encontrado</h3>
            <p className="text-muted-foreground mb-4">
              {searchQuery
                ? "Tente ajustar sua busca"
                : "Faça upload de arquivos para começar"}
            </p>
            {!searchQuery && (
              <ObjectUploader
                maxNumberOfFiles={5}
                maxFileSize={52428800}
                onGetUploadParameters={handleGetUploadParameters}
                onComplete={handleUploadComplete}
              >
                <Upload className="size-4 mr-2" />
                Fazer Upload
              </ObjectUploader>
            )}
          </div>
        ) : (
          <div className="border rounded-lg overflow-hidden">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead className="w-12"></TableHead>
                  <TableHead>Nome</TableHead>
                  <TableHead className="hidden md:table-cell">Tamanho</TableHead>
                  <TableHead className="hidden lg:table-cell">Projeto/Cliente</TableHead>
                  <TableHead className="hidden md:table-cell">Data</TableHead>
                  <TableHead className="w-12"></TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredFiles.map((file) => (
                  <TableRow
                    key={file.id}
                    className="cursor-pointer hover:bg-muted/50"
                    data-testid={`file-row-${file.id}`}
                  >
                    <TableCell>
                      {getFileIcon(file.mimeType)}
                    </TableCell>
                    <TableCell>
                      <button
                        onClick={() => handleDownload(file)}
                        className="text-left hover:underline font-medium"
                        data-testid={`file-download-${file.id}`}
                      >
                        {file.originalName}
                      </button>
                    </TableCell>
                    <TableCell className="hidden md:table-cell text-muted-foreground">
                      {formatFileSize(file.fileSize)}
                    </TableCell>
                    <TableCell className="hidden lg:table-cell text-muted-foreground">
                      {getProjectName(file.projectId) || getClientName(file.clientId) || "-"}
                    </TableCell>
                    <TableCell className="hidden md:table-cell text-muted-foreground">
                      {format(new Date(file.createdAt), "dd/MM/yyyy", { locale: ptBR })}
                    </TableCell>
                    <TableCell>
                      <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                          <Button variant="ghost" size="icon" data-testid={`file-actions-${file.id}`}>
                            <MoreHorizontal className="size-4" />
                          </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                          <DropdownMenuItem onClick={() => handleDownload(file)} data-testid={`file-action-download-${file.id}`}>
                            <Download className="size-4 mr-2" />
                            Download
                          </DropdownMenuItem>
                          <DropdownMenuItem
                            onClick={() => setDeleteFile(file)}
                            className="text-destructive focus:text-destructive"
                            data-testid={`file-action-delete-${file.id}`}
                          >
                            <Trash2 className="size-4 mr-2" />
                            Excluir
                          </DropdownMenuItem>
                        </DropdownMenuContent>
                      </DropdownMenu>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        )}
      </div>

      <DeleteModal
        open={!!deleteFile}
        onOpenChange={(open) => !open && setDeleteFile(null)}
        onConfirm={() => deleteFile && deleteFileMutation.mutate(deleteFile.id)}
        title="Excluir arquivo"
        description={`Tem certeza que deseja excluir "${deleteFile?.originalName}"? Esta ação não pode ser desfeita.`}
        isLoading={deleteFileMutation.isPending}
      />
    </AppShell>
  );
}
