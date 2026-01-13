import { useMemo, useState } from "react";
import { useAdvProcessoses, useDeleteAdvProcessos } from "@/lib/abp/hooks/useAdvProcessoses";
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

interface AdvProcessosListProps {
  onEdit: (item: any) => void;
}

export function AdvProcessosList({ onEdit }: AdvProcessosListProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const { data, isLoading, isError } = useAdvProcessoses({
    filter: searchTerm,
  });
  const deleteMutation = useDeleteAdvProcessos();

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this advprocessos?")) {
      try {
        await deleteMutation.mutateAsync(id);
        toast.success("advProcessos deleted successfully");
      } catch (error: any) {
        toast.error(error.message || "Failed to delete advprocessos");
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
              placeholder="Search advprocessoses..."
              className="pl-8"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        <Table>
          <TableHeader>
            <TableRow>
              
              <TableHead>idProcesso</TableHead>
              
              <TableHead>idCliente</TableHead>
              
              <TableHead>idUsuarioInclusao</TableHead>
              
              <TableHead>idEscritorioOrigem</TableHead>
              
              <TableHead>idEscritorioResponsavel</TableHead>
              
              <TableHead>idAutorPeticao</TableHead>
              
              <TableHead>idResponsavel</TableHead>
              
              <TableHead>sintese</TableHead>
              
              <TableHead>numero</TableHead>
              
              <TableHead>dataDistribuicao</TableHead>
              
              <TableHead>idStatus</TableHead>
              
              <TableHead>idNatureza</TableHead>
              
              <TableHead>idTipo</TableHead>
              
              <TableHead>estado</TableHead>
              
              <TableHead>cidade</TableHead>
              
              <TableHead>idFase</TableHead>
              
              <TableHead>idRelevancia</TableHead>
              
              <TableHead>idProbabilidade</TableHead>
              
              <TableHead>valorCausa</TableHead>
              
              <TableHead>valorHonorarios</TableHead>
              
              <TableHead>valorHonorariosTipo</TableHead>
              
              <TableHead>observacoes</TableHead>
              
              <TableHead>idSentenca</TableHead>
              
              <TableHead>dataSentenca</TableHead>
              
              <TableHead>alvara</TableHead>
              
              <TableHead>valorDeferido</TableHead>
              
              <TableHead>dataEncerramento</TableHead>
              
              <TableHead>ativo</TableHead>
              
              <TableHead>tsInclusao</TableHead>
              
              <TableHead>tsAlteracao</TableHead>
              
              <TableHead>idOrgao</TableHead>
              
              <TableHead>idInstancia</TableHead>
              
              <TableHead>idVara</TableHead>
              
              <TableHead>recurso</TableHead>
              
              <TableHead>recursoIdSentenca</TableHead>
              
              <TableHead>recursoDataSentenca</TableHead>
              
              <TableHead>alvaraPendente</TableHead>
              
              <TableHead>alvaraPendenteDesde</TableHead>
              
              <TableHead>historicoNumeros</TableHead>
              
              <TableHead>recebeAcordo</TableHead>
              
              <TableHead>recebeRPV</TableHead>
              
              <TableHead>recebePrecatorio</TableHead>
              
              <TableHead>recebeAlvara</TableHead>
              
              <TableHead>recebeBanco</TableHead>
              
              <TableHead>recebeDataLiberacao</TableHead>
              
              <TableHead>pendOutrosValores</TableHead>
              
              <TableHead>pendOutrosValoresDataEncerramento</TableHead>
              
              <TableHead>pendOutrosValoresDeferido</TableHead>
              
              <TableHead>pendOutrosValoresValorDeferido</TableHead>
              
              <TableHead>acaoColetiva</TableHead>
              
              <TableHead>temResponsavel</TableHead>
              
              <TableHead>nomeResponsavel</TableHead>
              
              <TableHead>cpfResponsavel</TableHead>
              
              <TableHead>imposto</TableHead>
              
              <TableHead>tarifa</TableHead>
              
              <TableHead>complementoPositivo</TableHead>
              
              <TableHead>RPV</TableHead>
              
              <TableHead>bancarioBanco</TableHead>
              
              <TableHead>bancarioTipoConta</TableHead>
              
              <TableHead>bancarioAgencia</TableHead>
              
              <TableHead>bancarioConta</TableHead>
              
              <TableHead>bancarioFavorecido</TableHead>
              
              <TableHead>bancarioCpf</TableHead>
              
              <TableHead>nomeReu</TableHead>
              
              <TableHead>sucumbencia</TableHead>
              
              <TableHead>idConta</TableHead>
              
              <TableHead>dataLiberacaoValorDeferido</TableHead>
              
              <TableHead>boleto</TableHead>
              
              <TableHead>precatorio</TableHead>
              
              <TableHead>emitir</TableHead>
              
              <TableHead>emitido</TableHead>
              
              <TableHead>formaRecebimento</TableHead>
              
              <TableHead>bancarioBancoId</TableHead>
              
              <TableHead>dataPrevisaoRepasseCliente</TableHead>
              
              <TableHead>honorariosTextoFicha</TableHead>
              
              <TableHead>nfComComplementoPositivo</TableHead>
              
              <TableHead>valorHonorariosDestaque</TableHead>
              
              <TableHead>valorHonorariosDestaqueTipo</TableHead>
              
              <TableHead>dataPrevisaoHonorariosDestaque</TableHead>
              
              <TableHead>idContaPagar</TableHead>
              
              <TableHead>bancarioPerc</TableHead>
              
              <TableHead>dataPrevistaClienteReceber</TableHead>
              
              <TableHead>sucumbenciaAdd</TableHead>
              
              <TableHead>sucumbenciaAddData</TableHead>
              
              <TableHead>sucumbenciaAddIdBanco</TableHead>
              
              <TableHead>saldoDevedor</TableHead>
              
              <TableHead>herdeirosTipoValor</TableHead>
              
              <TableHead>nrParcelasProcesso</TableHead>
              
              <TableHead>nrParcelasSomenteSucumbencia</TableHead>
              
              <TableHead>preProcesso</TableHead>
              
              <TableHead>preProcessoPasta</TableHead>
              
              <TableHead>preProcessoDataCriacao</TableHead>
              
              <TableHead>preProcessoDataPrevista</TableHead>
              
              <TableHead>preProcessoDataRealizada</TableHead>
              
              <TableHead>preProcessoIdStatus</TableHead>
              
              <TableHead>tsConversao</TableHead>
              
              <TableHead>perdido</TableHead>
              
              <TableHead>tsPerdido</TableHead>
              
              <TableHead>idMotivoPerda</TableHead>
              
              <TableHead>convertido</TableHead>
              
              <TableHead>clientePrimeiraVez</TableHead>
              
              <TableHead>preProcessoIdTipo</TableHead>
              
              <TableHead>tarifaParcelas</TableHead>
              
              <TableHead>idOrigem</TableHead>
              
              <TableHead>dataEntrada</TableHead>
              
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data?.items?.map((item: any) => (
              <TableRow key={item.id}>
                
                <TableCell>
                  
                  {item.idProcesso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idCliente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idUsuarioInclusao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idEscritorioOrigem}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idEscritorioResponsavel}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idAutorPeticao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idResponsavel}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sintese}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.numero}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataDistribuicao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idStatus}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idNatureza}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idTipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.estado}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idFase}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idRelevancia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idProbabilidade}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorCausa}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorHonorarios}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorHonorariosTipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.observacoes}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idSentenca}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataSentenca}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.alvara ? "default" : "secondary"}>
                    {item.alvara ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.valorDeferido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataEncerramento}
                  
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
                  
                  {item.idOrgao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idInstancia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idVara}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recurso ? "default" : "secondary"}>
                    {item.recurso ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.recursoIdSentenca}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.recursoDataSentenca}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.alvaraPendente ? "default" : "secondary"}>
                    {item.alvaraPendente ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.alvaraPendenteDesde}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.historicoNumeros}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recebeAcordo ? "default" : "secondary"}>
                    {item.recebeAcordo ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recebeRPV ? "default" : "secondary"}>
                    {item.recebeRPV ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recebePrecatorio ? "default" : "secondary"}>
                    {item.recebePrecatorio ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.recebeAlvara ? "default" : "secondary"}>
                    {item.recebeAlvara ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.recebeBanco}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.recebeDataLiberacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.pendOutrosValores ? "default" : "secondary"}>
                    {item.pendOutrosValores ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.pendOutrosValoresDataEncerramento}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.pendOutrosValoresDeferido ? "default" : "secondary"}>
                    {item.pendOutrosValoresDeferido ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.pendOutrosValoresValorDeferido}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.acaoColetiva ? "default" : "secondary"}>
                    {item.acaoColetiva ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.temResponsavel ? "default" : "secondary"}>
                    {item.temResponsavel ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nomeResponsavel}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.cpfResponsavel}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.imposto}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tarifa}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.complementoPositivo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.rPV}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioBanco}
                  
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
                  
                  {item.bancarioFavorecido}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioCpf}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nomeReu}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.sucumbencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idConta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataLiberacaoValorDeferido}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.boleto ? "default" : "secondary"}>
                    {item.boleto ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.precatorio}
                  
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
                  
                  {item.formaRecebimento}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioBancoId}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrevisaoRepasseCliente}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.honorariosTextoFicha}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.nfComComplementoPositivo ? "default" : "secondary"}>
                    {item.nfComComplementoPositivo ? "Yes" : "No"}
                  </Badge>
                  
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
                  
                  {item.idContaPagar}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.bancarioPerc}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataPrevistaClienteReceber}
                  
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
                  
                  {item.saldoDevedor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.herdeirosTipoValor}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nrParcelasProcesso}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.nrParcelasSomenteSucumbencia}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.preProcesso ? "default" : "secondary"}>
                    {item.preProcesso ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.preProcessoPasta}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.preProcessoDataCriacao}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.preProcessoDataPrevista}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.preProcessoDataRealizada}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.preProcessoIdStatus}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsConversao ? new Date(item.tsConversao).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.perdido ? "default" : "secondary"}>
                    {item.perdido ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tsPerdido ? new Date(item.tsPerdido).toLocaleDateString() : "-"}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idMotivoPerda}
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.convertido ? "default" : "secondary"}>
                    {item.convertido ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  <Badge variant={item.clientePrimeiraVez ? "default" : "secondary"}>
                    {item.clientePrimeiraVez ? "Yes" : "No"}
                  </Badge>
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.preProcessoIdTipo}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.tarifaParcelas}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.idOrigem}
                  
                </TableCell>
                
                <TableCell>
                  
                  {item.dataEntrada}
                  
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
