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
  import { useCreateAdvAgeTiposCompromissos, useUpdateAdvAgeTiposCompromissos } from "@/lib/abp/hooks/useAdvAgeTiposCompromissoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idTipoCompromisso: z.any(),
  
    titulo: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    recebimentoProcesso: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvAgeTiposCompromissosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvAgeTiposCompromissosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvAgeTiposCompromissosFormProps) {
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

  const createMutation = useCreateAdvAgeTiposCompromissos();
const updateMutation = useUpdateAdvAgeTiposCompromissos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advAgeTiposCompromissos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advAgeTiposCompromissos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advagetiposcompromissos:", error);
    toast.error(error.message || "Failed to save advagetiposcompromissos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advAgeTiposCompromissos": "Create advAgeTiposCompromissos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advagetiposcompromissos." : "Fill in the details to create a new advagetiposcompromissos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idTipoCompromisso" > idTipoCompromisso</Label>

<Input id="idTipoCompromisso" type = "number" step = "any" {...register("idTipoCompromisso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

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
  <Label htmlFor="recebimentoProcesso" > recebimentoProcesso</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebimentoProcesso"
checked = { watch("recebimentoProcesso") }
onCheckedChange = {(checked) => setValue("recebimentoProcesso", !!checked)}
              />
  < label htmlFor = "recebimentoProcesso" className = "text-sm font-normal" >
    { watch("recebimentoProcesso") ?"Enabled": "Disabled" }
    </label>
    </div>

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
