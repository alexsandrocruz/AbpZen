import { useMemo, useState } from "react";
import { useAdvProcessosHonorarioses, useDeleteAdvProcessosHonorarios } from "@/lib/abp/hooks/useAdvProcessosHonorarioses";
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

interface AdvProcessosHonorariosListProps {
  onEdit: (item: any) => void;
}

export function AdvProcessosHonorariosList({ onEdit }: AdvProcessosHonorariosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvProcessosHonorarioses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvProcessosHonorarios();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advprocessoshonorarios?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advProcessosHonorarios deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advprocessoshonorarios");
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
              placeholder="Search advprocessoshonorarioses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idHonorario</TableHead>
              
              <TableHead>idProcesso</TableHead>
              
              <TableHead>dataPrevistaClienteReceber</TableHead>
              
              <TableHead>rpv</TableHead>
              
              <TableHead>precatorio</TableHead>
              
              <TableHead>nrParcelasProcesso</TableHead>
              
              <TableHead>valorHonorarios</TableHead>
              
              <TableHead>valorHonorariosTipo</TableHead>
              
              <TableHead>honorariosTextoFicha</TableHead>
              
              <TableHead>valorHonorariosDestaque</TableHead>
              
              <TableHead>valorHonorariosDestaqueTipo</TableHead>
              
              <TableHead>dataPrevisaoHonorariosDestaque</TableHead>
              
              <TableHead>imposto</TableHead>
              
              <TableHead>complementoPositivo</TableHead>
              
              <TableHead>sucumbencia</TableHead>
              
              <TableHead>saldoDevedor</TableHead>
              
              <TableHead>valorDeferido</TableHead>
              
              <TableHead>dataLiberacaoValorDeferido</TableHead>
              
              <TableHead>idConta</TableHead>
              
              <TableHead>dataPrevisaoRepasseCliente</TableHead>
              
              <TableHead>idContaPagar</TableHead>
              
              <TableHead>formaRecebimento</TableHead>
              
              <TableHead>nrParcelasSomenteSucumbencia</TableHead>
              
              <TableHead>sucumbenciaAdd</TableHead>
              
              <TableHead>sucumbenciaAddData</TableHead>
              
              <TableHead>sucumbenciaAddIdBanco</TableHead>
              
              <TableHead>boleto</TableHead>
              
              <TableHead>emitir</TableHead>
              
              <TableHead>emitido</TableHead>
              
              <TableHead>nfComComplementoPositivo</TableHead>
              
              <TableHead>bancarioCpf</TableHead>
              
              <TableHead>herdeirosTipoValor</TableHead>
              
              <TableHead>bancarioPerc</TableHead>
              
              <TableHead>tarifa</TableHead>
              
              <TableHead>tarifaParcelas</TableHead>
              
              <TableHead>bancarioFavorecido</TableHead>
              
              <TableHead>bancarioBancoId</TableHead>
              
              <TableHead>bancarioTipoConta</TableHead>
              
              <TableHead>bancarioAgencia</TableHead>
              
              <TableHead>bancarioConta</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>incluidoPor</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>alteradoPor</TableHead>
              
              <TableHead>valorHonorariosDestaqueSomente</TableHead>
              
              <TableHead>valorHonorariosDestaqueTipoSomente</TableHead>
              
              <TableHead>dataPrevisaoHonorariosDestaqueSomente</TableHead>
              
              <TableHead>idBancoDestaqueSomente</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idHonorario}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idProcesso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrevistaClienteReceber}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.rpv}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.precatorio}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nrParcelasProcesso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorHonorarios}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorHonorariosTipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.honorariosTextoFicha}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorHonorariosDestaque}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorHonorariosDestaqueTipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrevisaoHonorariosDestaque}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.imposto}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.complementoPositivo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sucumbencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.saldoDevedor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorDeferido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataLiberacaoValorDeferido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idConta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrevisaoRepasseCliente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idContaPagar}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.formaRecebimento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nrParcelasSomenteSucumbencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sucumbenciaAdd}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sucumbenciaAddData}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sucumbenciaAddIdBanco}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.boleto ? "default" : "secondary"}>
                    {item.boleto ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.emitir}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.emitido ? "default" : "secondary"}>
                    {item.emitido ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.nfComComplementoPositivo ? "default" : "secondary"}>
                    {item.nfComComplementoPositivo ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioCpf}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.herdeirosTipoValor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioPerc}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tarifa}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tarifaParcelas}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioFavorecido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioBancoId}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioTipoConta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioAgencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioConta}
                  
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
                  
                  {item.incluidoPor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsAlteracao ? new Date(item.tsAlteracao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.alteradoPor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorHonorariosDestaqueSomente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorHonorariosDestaqueTipoSomente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrevisaoHonorariosDestaqueSomente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idBancoDestaqueSomente}
                  
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
