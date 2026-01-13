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
  import { useCreateFinExtrato, useUpdateFinExtrato } from "@/lib/abp/hooks/useFinExtratos";
import { toast } from "sonner";

const formSchema = z.object({
  
    idExtrato: z.any(),
  
    idConta: z.any(),
  
    idLancamento: z.any(),
  
    transferencia: z.any(),
  
    idExtratoRel: z.any(),
  
    data: z.any(),
  
    descricao: z.any(),
  
    credito: z.any(),
  
    debito: z.any(),
  
    conferido: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idUsuarioInclusao: z.any(),
  
    idUsuarioAlteracao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinExtratoFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinExtratoForm({
  isOpen,
  onClose,
  initialValues,
}: FinExtratoFormProps) {
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

  const createMutation = useCreateFinExtrato();
const updateMutation = useUpdateFinExtrato();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finExtrato updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finExtrato created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save finextrato:", error);
    toast.error(error.message || "Failed to save finextrato");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finExtrato": "Create finExtrato" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the finextrato." : "Fill in the details to create a new finextrato." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idExtrato" > idExtrato</Label>

<Input id="idExtrato" type = "number" step = "any" {...register("idExtrato") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idConta" > idConta * </Label>

<Input id="idConta" type = "number" step = "any" {...register("idConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idLancamento" > idLancamento</Label>

<Input id="idLancamento" type = "number" step = "any" {...register("idLancamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="transferencia" > transferencia</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="transferencia"
checked = { watch("transferencia") }
onCheckedChange = {(checked) => setValue("transferencia", !!checked)}
              />
  < label htmlFor = "transferencia" className = "text-sm font-normal" >
    { watch("transferencia") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="idExtratoRel" > idExtratoRel</Label>

<Input id="idExtratoRel" type = "number" step = "any" {...register("idExtratoRel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="data" > data</Label>

<Input id="data" {...register("data") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="credito" > credito</Label>

<Input id="credito" type = "number" step = "any" {...register("credito") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="debito" > debito</Label>

<Input id="debito" type = "number" step = "any" {...register("debito") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="conferido" > conferido</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="conferido"
checked = { watch("conferido") }
onCheckedChange = {(checked) => setValue("conferido", !!checked)}
              />
  < label htmlFor = "conferido" className = "text-sm font-normal" >
    { watch("conferido") ?"Enabled": "Disabled" }
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
  <Label htmlFor="idUsuarioInclusao" > idUsuarioInclusao</Label>

<Input id="idUsuarioInclusao" type = "number" step = "any" {...register("idUsuarioInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuarioAlteracao" > idUsuarioAlteracao</Label>

<Input id="idUsuarioAlteracao" type = "number" step = "any" {...register("idUsuarioAlteracao") } />

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
