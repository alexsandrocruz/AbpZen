import { useMemo, useState } from "react";
import { useUsuUsuarioses, useDeleteUsuUsuarios } from "@/lib/abp/hooks/useUsuUsuarioses";
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
import { Search, MoreHorizontal, Pencil, Trash2, Loader2 } from "lucide-react";
import { toast } from "sonner";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

interface UsuUsuariosListProps {
  onEdit: (item: any) => void;
}

export function UsuUsuariosList({ onEdit }: UsuUsuariosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useUsuUsuarioses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteUsuUsuarios();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this usuusuarios?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("usuUsuarios deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete usuusuarios");
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
        <div className="p-4 border-b">
          <div className="relative max-w-sm">
            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search usuusuarioses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idUsuario</TableHead>
              
              <TableHead>nome</TableHead>
              
              <TableHead>sobrenome</TableHead>
              
              <TableHead>idArea</TableHead>
              
              <TableHead>idCargo</TableHead>
              
              <TableHead>login</TableHead>
              
              <TableHead>senha</TableHead>
              
              <TableHead>diaNascimento</TableHead>
              
              <TableHead>mesNascimento</TableHead>
              
              <TableHead>anoNascimento</TableHead>
              
              <TableHead>email</TableHead>
              
              <TableHead>telCelular</TableHead>
              
              <TableHead>telFixo</TableHead>
              
              <TableHead>endereco</TableHead>
              
              <TableHead>numero</TableHead>
              
              <TableHead>complemento</TableHead>
              
              <TableHead>bairro</TableHead>
              
              <TableHead>cep</TableHead>
              
              <TableHead>estado</TableHead>
              
              <TableHead>cidade</TableHead>
              
              <TableHead>cpf</TableHead>
              
              <TableHead>banco</TableHead>
              
              <TableHead>agencia</TableHead>
              
              <TableHead>conta</TableHead>
              
              <TableHead>foto</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>cor</TableHead>
              
              <TableHead>dashboardInicial</TableHead>
              
              <TableHead>tokenPhoneApp</TableHead>
              
              <TableHead>estadoCivil</TableHead>
              
              <TableHead>nrFilhos</TableHead>
              
              <TableHead>idadeFilhoMenor</TableHead>
              
              <TableHead>formacaoAcademica</TableHead>
              
              <TableHead>regiao</TableHead>
              
              <TableHead>idSuperior</TableHead>
              
              <TableHead>master</TableHead>
              
              <TableHead>mediaConsumoLitro</TableHead>
              
              <TableHead>distanciasIguais</TableHead>
              
              <TableHead>chaveChamados</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idUsuario}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nome}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sobrenome}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idArea}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCargo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.login}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.senha}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.diaNascimento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.mesNascimento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.anoNascimento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.email}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telCelular}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telFixo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.endereco}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.numero}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.complemento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bairro}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cep}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.estado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cpf}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.banco}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.agencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.conta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.foto}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.ativo ? "default" : "secondary"}>
                    {item.ativo ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsInclusao ? new Date(item.tsInclusao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsAlteracao ? new Date(item.tsAlteracao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dashboardInicial}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tokenPhoneApp}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.estadoCivil}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nrFilhos}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idadeFilhoMenor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.formacaoAcademica}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.regiao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idSuperior}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.master ? "default" : "secondary"}>
                    {item.master ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.mediaConsumoLitro}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.distanciasIguais}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.chaveChamados}
                  
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
      </CardContent>
    </Card>
  );
}
