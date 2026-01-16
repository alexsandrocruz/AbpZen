import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateOpoOrcamentos, useUpdateOpoOrcamentos } from "@/lib/abp/hooks/useOpoOrcamentoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idOrcamento: z.any(),
  
    idOportunidade: z.any(),
  
    titulo: z.any(),
  
    dataCriacao: z.any(),
  
    valor: z.any(),
  
    arquivo: z.any(),
  
    aceito: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    dataValidade: z.any(),
  
    valorMensal: z.any(),
  
    comArquivo: z.any(),
  
    comProduto: z.any(),
  
    comProdutoTerceiro: z.any(),
  
    comServico: z.any(),
  
    valorDesconto: z.any(),
  
    valorAcrescimo: z.any(),
  
    valorFrete: z.any(),
  
    informacoes: z.any(),
  
    descontoPercentual: z.any(),
  
    valorItens: z.any(),
  
    idCondicaoPagamento: z.any(),
  
    dataPrevistaEntrega: z.any(),
  
    moeda: z.any(),
  
    valorConversao: z.any(),
  
    imprimeMoedaAdd: z.any(),
  
    valorDescontoMensal: z.any(),
  
    valorAcrescimoMensal: z.any(),
  
    valorFreteMensal: z.any(),
  
    descontoPercentualMensal: z.any(),
  
    valorItensMensal: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface OpoOrcamentosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function OpoOrcamentosForm({
  isOpen,
  onClose,
  initialValues,
}: OpoOrcamentosFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateOpoOrcamentos();
const updateMutation = useUpdateOpoOrcamentos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("opoOrcamentos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("opoOrcamentos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save opoorcamentos:", error);
    toast.error(error.message || "Failed to save opoorcamentos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit opoOrcamentos": "Create opoOrcamentos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the opoorcamentos." : "Fill in the details to create a new opoorcamentos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idOrcamento" > idOrcamento</Label>

<Input id="idOrcamento" type = "number" step = "any" {...register("idOrcamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idOportunidade" > idOportunidade * </Label>

<Input id="idOportunidade" type = "number" step = "any" {...register("idOportunidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataCriacao" > dataCriacao</Label>

<Input id="dataCriacao" {...register("dataCriacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valor" > valor</Label>

<Input id="valor" type = "number" step = "any" {...register("valor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="arquivo" > arquivo</Label>

<Input id="arquivo" {...register("arquivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="aceito" > aceito</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="aceito"
checked = { watch("aceito") }
onCheckedChange = {(checked) => setValue("aceito", !!checked)}
              />
  < label htmlFor = "aceito" className = "text-sm font-normal" >
    { watch("aceito") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ativo"
checked = { watch("ativo") }
onCheckedChange = {(checked) => setValue("ativo", !!checked)}
              />
  < label htmlFor = "ativo" className = "text-sm font-normal" >
    { watch("ativo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataValidade" > dataValidade</Label>

<Input id="dataValidade" {...register("dataValidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorMensal" > valorMensal</Label>

<Input id="valorMensal" type = "number" step = "any" {...register("valorMensal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="comArquivo" > comArquivo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="comArquivo"
checked = { watch("comArquivo") }
onCheckedChange = {(checked) => setValue("comArquivo", !!checked)}
              />
  < label htmlFor = "comArquivo" className = "text-sm font-normal" >
    { watch("comArquivo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="comProduto" > comProduto</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="comProduto"
checked = { watch("comProduto") }
onCheckedChange = {(checked) => setValue("comProduto", !!checked)}
              />
  < label htmlFor = "comProduto" className = "text-sm font-normal" >
    { watch("comProduto") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="comProdutoTerceiro" > comProdutoTerceiro</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="comProdutoTerceiro"
checked = { watch("comProdutoTerceiro") }
onCheckedChange = {(checked) => setValue("comProdutoTerceiro", !!checked)}
              />
  < label htmlFor = "comProdutoTerceiro" className = "text-sm font-normal" >
    { watch("comProdutoTerceiro") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="comServico" > comServico</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="comServico"
checked = { watch("comServico") }
onCheckedChange = {(checked) => setValue("comServico", !!checked)}
              />
  < label htmlFor = "comServico" className = "text-sm font-normal" >
    { watch("comServico") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="valorDesconto" > valorDesconto</Label>

<Input id="valorDesconto" type = "number" step = "any" {...register("valorDesconto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorAcrescimo" > valorAcrescimo</Label>

<Input id="valorAcrescimo" type = "number" step = "any" {...register("valorAcrescimo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorFrete" > valorFrete</Label>

<Input id="valorFrete" type = "number" step = "any" {...register("valorFrete") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="informacoes" > informacoes</Label>

<Input id="informacoes" {...register("informacoes") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descontoPercentual" > descontoPercentual</Label>

<Input id="descontoPercentual" type = "number" step = "any" {...register("descontoPercentual") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorItens" > valorItens</Label>

<Input id="valorItens" type = "number" step = "any" {...register("valorItens") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCondicaoPagamento" > idCondicaoPagamento</Label>

<Input id="idCondicaoPagamento" type = "number" step = "any" {...register("idCondicaoPagamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrevistaEntrega" > dataPrevistaEntrega</Label>

<Input id="dataPrevistaEntrega" {...register("dataPrevistaEntrega") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="moeda" > moeda</Label>

<Input id="moeda" {...register("moeda") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorConversao" > valorConversao</Label>

<Input id="valorConversao" type = "number" step = "any" {...register("valorConversao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="imprimeMoedaAdd" > imprimeMoedaAdd</Label>

<Input id="imprimeMoedaAdd" {...register("imprimeMoedaAdd") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorDescontoMensal" > valorDescontoMensal</Label>

<Input id="valorDescontoMensal" type = "number" step = "any" {...register("valorDescontoMensal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorAcrescimoMensal" > valorAcrescimoMensal</Label>

<Input id="valorAcrescimoMensal" type = "number" step = "any" {...register("valorAcrescimoMensal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorFreteMensal" > valorFreteMensal</Label>

<Input id="valorFreteMensal" type = "number" step = "any" {...register("valorFreteMensal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descontoPercentualMensal" > descontoPercentualMensal</Label>

<Input id="descontoPercentualMensal" type = "number" step = "any" {...register("descontoPercentualMensal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorItensMensal" > valorItensMensal</Label>

<Input id="valorItensMensal" type = "number" step = "any" {...register("valorItensMensal") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
