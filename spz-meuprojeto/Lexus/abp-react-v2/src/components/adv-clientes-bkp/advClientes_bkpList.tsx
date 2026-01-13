import { useMemo, useState } from "react";
import { useAdvClientes_bkps, useDeleteAdvClientes_bkp } from "@/lib/abp/hooks/useAdvClientes_bkps";
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

interface AdvClientes_bkpListProps {
  onEdit: (item: any) => void;
}

export function AdvClientes_bkpList({ onEdit }: AdvClientes_bkpListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvClientes_bkps({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvClientes_bkp();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advclientes_bkp?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advClientes_bkp deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advclientes_bkp");
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
              placeholder="Search advclientes_bkps..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idCliente</TableHead>
              
              <TableHead>apelido</TableHead>
              
              <TableHead>idGrupo</TableHead>
              
              <TableHead>idSituacao</TableHead>
              
              <TableHead>nome</TableHead>
              
              <TableHead>email</TableHead>
              
              <TableHead>telCelular</TableHead>
              
              <TableHead>telCelularObs</TableHead>
              
              <TableHead>telFixo</TableHead>
              
              <TableHead>telFixoObs</TableHead>
              
              <TableHead>dataNascimento</TableHead>
              
              <TableHead>cpf</TableHead>
              
              <TableHead>rg</TableHead>
              
              <TableHead>ctps</TableHead>
              
              <TableHead>endereco</TableHead>
              
              <TableHead>numero</TableHead>
              
              <TableHead>complemento</TableHead>
              
              <TableHead>bairro</TableHead>
              
              <TableHead>cep</TableHead>
              
              <TableHead>estado</TableHead>
              
              <TableHead>cidade</TableHead>
              
              <TableHead>dataIngresso</TableHead>
              
              <TableHead>observacoes</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>naturalEstado</TableHead>
              
              <TableHead>naturalCidade</TableHead>
              
              <TableHead>nomeDaMae</TableHead>
              
              <TableHead>dib</TableHead>
              
              <TableHead>dibData</TableHead>
              
              <TableHead>dibIdTipoBeneficio</TableHead>
              
              <TableHead>idCargo</TableHead>
              
              <TableHead>telCelular2</TableHead>
              
              <TableHead>telCelular2Obs</TableHead>
              
              <TableHead>telFixo2</TableHead>
              
              <TableHead>telFixo2Obs</TableHead>
              
              <TableHead>cnpj</TableHead>
              
              <TableHead>ie</TableHead>
              
              <TableHead>idFornecedor</TableHead>
              
              <TableHead>incluidoPor</TableHead>
              
              <TableHead>inssAgendado</TableHead>
              
              <TableHead>inssData</TableHead>
              
              <TableHead>inssIdTipoBeneficio</TableHead>
              
              <TableHead>inssIdPosto</TableHead>
              
              <TableHead>inssResultado</TableHead>
              
              <TableHead>prospect</TableHead>
              
              <TableHead>idLocalAtendido</TableHead>
              
              <TableHead>whatsapp</TableHead>
              
              <TableHead>pastaFTP</TableHead>
              
              <TableHead>inssResponsavel</TableHead>
              
              <TableHead>responsavelPendencia</TableHead>
              
              <TableHead>comoChegou</TableHead>
              
              <TableHead>inssProtocolo</TableHead>
              
              <TableHead>inssTsInclusao</TableHead>
              
              <TableHead>inssIdUsuarioInclusao</TableHead>
              
              <TableHead>foto</TableHead>
              
              <TableHead>followBloqueadoAte</TableHead>
              
              <TableHead>falecido</TableHead>
              
              <TableHead>senhaINSSDigital</TableHead>
              
              <TableHead>idPrioridade</TableHead>
              
              <TableHead>instagram</TableHead>
              
              <TableHead>rgOrgaoExp</TableHead>
              
              <TableHead>nacionalidade</TableHead>
              
              <TableHead>estadocivil</TableHead>
              
              <TableHead>dcb</TableHead>
              
              <TableHead>dcbData</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idCliente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.apelido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idGrupo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idSituacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nome}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.email}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telCelular}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telCelularObs}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telFixo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telFixoObs}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataNascimento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cpf}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.rg}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.ctps}
                  
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
                  
                  {item.dataIngresso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.observacoes}
                  
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
                  
                  {item.naturalEstado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.naturalCidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nomeDaMae}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.dib ? "default" : "secondary"}>
                    {item.dib ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dibData}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dibIdTipoBeneficio}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCargo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telCelular2}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telCelular2Obs}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telFixo2}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.telFixo2Obs}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cnpj}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.ie}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idFornecedor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.incluidoPor}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.inssAgendado ? "default" : "secondary"}>
                    {item.inssAgendado ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssData}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssIdTipoBeneficio}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssIdPosto}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssResultado}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.prospect ? "default" : "secondary"}>
                    {item.prospect ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idLocalAtendido}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.whatsapp ? "default" : "secondary"}>
                    {item.whatsapp ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.pastaFTP}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssResponsavel}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.responsavelPendencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.comoChegou}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssProtocolo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssTsInclusao ? new Date(item.inssTsInclusao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.inssIdUsuarioInclusao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.foto}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.followBloqueadoAte}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.falecido ? "default" : "secondary"}>
                    {item.falecido ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.senhaINSSDigital}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idPrioridade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.instagram}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.rgOrgaoExp}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nacionalidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.estadocivil}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.dcb ? "default" : "secondary"}>
                    {item.dcb ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dcbData}
                  
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
