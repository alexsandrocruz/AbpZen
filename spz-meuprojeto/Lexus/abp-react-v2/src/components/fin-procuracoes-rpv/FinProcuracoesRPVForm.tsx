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
  import { useCreateFinProcuracoesRPV, useUpdateFinProcuracoesRPV } from "@/lib/abp/hooks/useFinProcuracoesRPVs";
import { toast } from "sonner";

const formSchema = z.object({
  
    idProcuracao: z.any(),
  
    idCliente: z.any(),
  
    idProcesso: z.any(),
  
    impressa: z.any(),
  
    tsImpressa: z.any(),
  
    assinada: z.any(),
  
    tsAssinatura: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinProcuracoesRPVFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinProcuracoesRPVForm({
  isOpen,
  onClose,
  initialValues,
}: FinProcuracoesRPVFormProps) {
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

  const createMutation = useCreateFinProcuracoesRPV();
const updateMutation = useUpdateFinProcuracoesRPV();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finProcuracoesRPV updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finProcuracoesRPV created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save finprocuracoesrpv:", error);
    toast.error(error.message || "Failed to save finprocuracoesrpv");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finProcuracoesRPV": "Create finProcuracoesRPV" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the finprocuracoesrpv." : "Fill in the details to create a new finprocuracoesrpv." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idProcuracao" > idProcuracao</Label>

<Input id="idProcuracao" type = "number" step = "any" {...register("idProcuracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente</Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="impressa" > impressa</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="impressa"
checked = { watch("impressa") }
onCheckedChange = {(checked) => setValue("impressa", !!checked)}
              />
  < label htmlFor = "impressa" className = "text-sm font-normal" >
    { watch("impressa") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsImpressa" > tsImpressa</Label>

<Input id="tsImpressa" type = "date" {...register("tsImpressa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="assinada" > assinada</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="assinada"
checked = { watch("assinada") }
onCheckedChange = {(checked) => setValue("assinada", !!checked)}
              />
  < label htmlFor = "assinada" className = "text-sm font-normal" >
    { watch("assinada") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAssinatura" > tsAssinatura</Label>

<Input id="tsAssinatura" type = "date" {...register("tsAssinatura") } />

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
