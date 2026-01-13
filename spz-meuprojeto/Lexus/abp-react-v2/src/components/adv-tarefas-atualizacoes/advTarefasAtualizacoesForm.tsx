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
  import { useCreateAdvTarefasAtualizacoes, useUpdateAdvTarefasAtualizacoes } from "@/lib/abp/hooks/useAdvTarefasAtualizacoeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idAtualizacaoTarefa: z.any(),
  
    idTarefa: z.any(),
  
    idCompromisso: z.any(),
  
    campo: z.any(),
  
    tsAlteracao: z.any(),
  
    dadoAnterior: z.any(),
  
    idUsuario: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvTarefasAtualizacoesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvTarefasAtualizacoesForm({
  isOpen,
  onClose,
  initialValues,
}: AdvTarefasAtualizacoesFormProps) {
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

  const createMutation = useCreateAdvTarefasAtualizacoes();
const updateMutation = useUpdateAdvTarefasAtualizacoes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advTarefasAtualizacoes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advTarefasAtualizacoes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advtarefasatualizacoes:", error);
    toast.error(error.message || "Failed to save advtarefasatualizacoes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advTarefasAtualizacoes": "Create advTarefasAtualizacoes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advtarefasatualizacoes." : "Fill in the details to create a new advtarefasatualizacoes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idAtualizacaoTarefa" > idAtualizacaoTarefa</Label>

<Input id="idAtualizacaoTarefa" type = "number" step = "any" {...register("idAtualizacaoTarefa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTarefa" > idTarefa</Label>

<Input id="idTarefa" type = "number" step = "any" {...register("idTarefa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCompromisso" > idCompromisso</Label>

<Input id="idCompromisso" type = "number" step = "any" {...register("idCompromisso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="campo" > campo</Label>

<Input id="campo" {...register("campo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dadoAnterior" > dadoAnterior</Label>

<Input id="dadoAnterior" {...register("dadoAnterior") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario</Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

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
