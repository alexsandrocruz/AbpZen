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
  import { useCreateFabFormasRecebimento, useUpdateFabFormasRecebimento } from "@/lib/abp/hooks/useFabFormasRecebimentos";
import { toast } from "sonner";

const formSchema = z.object({
  
    idFormaRecebimento: z.any(),
  
    titulo: z.any(),
  
    ordem: z.any(),
  
    padrao: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idCondicaoPagamento: z.any(),
  
    online: z.any(),
  
    tipo: z.any(),
  
    emailPagSeguro: z.any(),
  
    texto: z.any(),
  
    contasReceber: z.any(),
  
    vendas: z.any(),
  
    diasParaPrevisao: z.any(),
  
    valorDesconto: z.any(),
  
    descontoTipo: z.any(),
  
    recebimentoFuturo: z.any(),
  
    recebimentoFuturoDias: z.any(),
  
    recebimentoFuturoTaxa: z.any(),
  
    idConta: z.any(),
  
    idPlanoConta: z.any(),
  
    idCentroCusto: z.any(),
  
    idContaPagar: z.any(),
  
    idPlanoContaPagar: z.any(),
  
    idCentroCustoPagar: z.any(),
  
    idFormaPagar: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabFormasRecebimentoFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabFormasRecebimentoForm({
  isOpen,
  onClose,
  initialValues,
}: FabFormasRecebimentoFormProps) {
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

  const createMutation = useCreateFabFormasRecebimento();
const updateMutation = useUpdateFabFormasRecebimento();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabFormasRecebimento updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabFormasRecebimento created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fabformasrecebimento:", error);
    toast.error(error.message || "Failed to save fabformasrecebimento");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabFormasRecebimento": "Create fabFormasRecebimento" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fabformasrecebimento." : "Fill in the details to create a new fabformasrecebimento." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idFormaRecebimento" > idFormaRecebimento</Label>

<Input id="idFormaRecebimento" type = "number" step = "any" {...register("idFormaRecebimento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ordem" > ordem</Label>

<Input id="ordem" type = "number" step = "any" {...register("ordem") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="padrao" > padrao</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="padrao"
checked = { watch("padrao") }
onCheckedChange = {(checked) => setValue("padrao", !!checked)}
              />
  < label htmlFor = "padrao" className = "text-sm font-normal" >
    { watch("padrao") ?"Enabled": "Disabled" }
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
  <Label htmlFor="idCondicaoPagamento" > idCondicaoPagamento</Label>

<Input id="idCondicaoPagamento" type = "number" step = "any" {...register("idCondicaoPagamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="online" > online</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="online"
checked = { watch("online") }
onCheckedChange = {(checked) => setValue("online", !!checked)}
              />
  < label htmlFor = "online" className = "text-sm font-normal" >
    { watch("online") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tipo" > tipo</Label>

<Input id="tipo" {...register("tipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="emailPagSeguro" > emailPagSeguro</Label>

<Input id="emailPagSeguro" {...register("emailPagSeguro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="texto" > texto</Label>

<Input id="texto" {...register("texto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="contasReceber" > contasReceber</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="contasReceber"
checked = { watch("contasReceber") }
onCheckedChange = {(checked) => setValue("contasReceber", !!checked)}
              />
  < label htmlFor = "contasReceber" className = "text-sm font-normal" >
    { watch("contasReceber") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="vendas" > vendas</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="vendas"
checked = { watch("vendas") }
onCheckedChange = {(checked) => setValue("vendas", !!checked)}
              />
  < label htmlFor = "vendas" className = "text-sm font-normal" >
    { watch("vendas") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="diasParaPrevisao" > diasParaPrevisao</Label>

<Input id="diasParaPrevisao" type = "number" step = "any" {...register("diasParaPrevisao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorDesconto" > valorDesconto</Label>

<Input id="valorDesconto" type = "number" step = "any" {...register("valorDesconto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descontoTipo" > descontoTipo</Label>

<Input id="descontoTipo" {...register("descontoTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="recebimentoFuturo" > recebimentoFuturo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebimentoFuturo"
checked = { watch("recebimentoFuturo") }
onCheckedChange = {(checked) => setValue("recebimentoFuturo", !!checked)}
              />
  < label htmlFor = "recebimentoFuturo" className = "text-sm font-normal" >
    { watch("recebimentoFuturo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebimentoFuturoDias" > recebimentoFuturoDias</Label>

<Input id="recebimentoFuturoDias" type = "number" step = "any" {...register("recebimentoFuturoDias") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="recebimentoFuturoTaxa" > recebimentoFuturoTaxa</Label>

<Input id="recebimentoFuturoTaxa" type = "number" step = "any" {...register("recebimentoFuturoTaxa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idConta" > idConta</Label>

<Input id="idConta" type = "number" step = "any" {...register("idConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPlanoConta" > idPlanoConta</Label>

<Input id="idPlanoConta" type = "number" step = "any" {...register("idPlanoConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCentroCusto" > idCentroCusto</Label>

<Input id="idCentroCusto" type = "number" step = "any" {...register("idCentroCusto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idContaPagar" > idContaPagar</Label>

<Input id="idContaPagar" type = "number" step = "any" {...register("idContaPagar") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPlanoContaPagar" > idPlanoContaPagar</Label>

<Input id="idPlanoContaPagar" type = "number" step = "any" {...register("idPlanoContaPagar") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCentroCustoPagar" > idCentroCustoPagar</Label>

<Input id="idCentroCustoPagar" type = "number" step = "any" {...register("idCentroCustoPagar") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idFormaPagar" > idFormaPagar</Label>

<Input id="idFormaPagar" type = "number" step = "any" {...register("idFormaPagar") } />

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
