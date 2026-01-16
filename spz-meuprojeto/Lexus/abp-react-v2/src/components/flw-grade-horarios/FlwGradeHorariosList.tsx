import { useMemo, useState } from "react";
import { useFlwGradeHorarioses, useDeleteFlwGradeHorarios } from "@/lib/abp/hooks/useFlwGradeHorarioses";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Search, MoreHorizontal, Pencil, Trash2, Loader2, LayoutGrid, List } from "lucide-react";
import { toast } from "sonner";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { FlwGradeHorariosCard } from "./FlwGradeHorariosCard";

interface FlwGradeHorariosListProps {
  onEdit: (item: any) => void;
}

export function FlwGradeHorariosList({ onEdit }: FlwGradeHorariosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const { data, isLoading, isError } = useFlwGradeHorarioses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteFlwGradeHorarios();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this flwgradehorarios?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("flwGradeHorarios deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete flwgradehorarios");
      }
    }
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center p-12">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  return (
    <Card>
      <CardContent className="p-0">
        <div className="p-4 border-b flex items-center justify-between gap-4">
          <div className="relative max-w-sm flex-1">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search flwgradehorarioses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <div className="flex items-center gap-1 border rounded-md p-1 bg-muted/20">
            <Button 
              variant={viewMode === "grid" ? "secondary" : "ghost"} 
              size="icon"
              className="h-8 w-8"
              onClick={() => setViewMode("grid")}
            >
              <LayoutGrid className="h-4 w-4" />
            </Button>
            <Button 
              variant={viewMode === "list" ? "secondary" : "ghost"} 
              size="icon"
              className="h-8 w-8"
              onClick={() => setViewMode("list")}
            >
              <List className="h-4 w-4" />
            </Button>
          </div>
        </div>

        {viewMode === "list" ? (
          <Table>
            <TableHeader>
              <TableRow>
                
                <TableHead>idGrade</TableHead>
                
                <TableHead>idHistoricoTipo</TableHead>
                
                <TableHead>manhaHorarioInicial</TableHead>
                
                <TableHead>manhaIntervalo</TableHead>
                
                <TableHead>manhaQtde</TableHead>
                
                <TableHead>tardeHorarioInicial</TableHead>
                
                <TableHead>tardeIntervalo</TableHead>
                
                <TableHead>tardeQtde</TableHead>
                
                <TableHead>dom</TableHead>
                
                <TableHead>seg</TableHead>
                
                <TableHead>ter</TableHead>
                
                <TableHead>qua</TableHead>
                
                <TableHead>qui</TableHead>
                
                <TableHead>sex</TableHead>
                
                <TableHead>sab</TableHead>
                
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {data?.items?.map((item: any) => (
                <TableRow key={item.id}>
                  
                  <TableCell>
                    
                    {item.idGrade}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.idHistoricoTipo}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.manhaHorarioInicial}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.manhaIntervalo}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.manhaQtde}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.tardeHorarioInicial}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.tardeIntervalo}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    {item.tardeQtde}
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.dom ? "default" : "secondary"}>
                      {item.dom ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.seg ? "default" : "secondary"}>
                      {item.seg ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.ter ? "default" : "secondary"}>
                      {item.ter ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.qua ? "default" : "secondary"}>
                      {item.qua ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.qui ? "default" : "secondary"}>
                      {item.qui ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.sex ? "default" : "secondary"}>
                      {item.sex ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell>
                    
                    <Badge variant={item.sab ? "default" : "secondary"}>
                      {item.sab ? "Yes" : "No"}
                    </Badge>
                    
                  </TableCell>
                  
                  <TableCell className="text-right">
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="icon">
                          <MoreHorizontal className="h-4 w-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end">
                        <DropdownMenuLabel>Actions</DropdownMenuLabel>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem onClick={() => onEdit(item)}>
                          <Pencil className="mr-2 h-4 w-4" />
                          Edit
                        </DropdownMenuItem>
                        <DropdownMenuItem 
                          className="text-destructive"
                          onClick={() => handleDelete(item.id)}
                        >
                          <Trash2 className="mr-2 h-4 w-4" />
                          Delete
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </TableCell>
                </TableRow>
              ))}
              {(!data?.items || data.items.length === 0) && (
                <TableRow>
                  <TableCell colSpan={99} className="h-24 text-center">
                    No results found.
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        ) : (
          <div className="p-4">
            {(!data?.items || data.items.length === 0) ? (
              <div className="flex items-center justify-center h-24 text-muted-foreground">
                No results found.
              </div>
            ) : (
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                {data.items.map((item: any) => (
                  <FlwGradeHorariosCard
                    key={item.id}
                    item={item}
                    onEdit={onEdit}
                    onDelete={handleDelete}
                  />
                ))}
              </div>
            )}
          </div>
        )}
      </CardContent>
    </Card>
  );
}
