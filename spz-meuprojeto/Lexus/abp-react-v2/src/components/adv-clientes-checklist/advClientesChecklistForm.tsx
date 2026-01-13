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
  import { useCreateAdvClientesChecklist, useUpdateAdvClientesChecklist } from "@/lib/abp/hooks/useAdvClientesChecklists";
import { toast } from "sonner";

const formSchema = z.object({
  
    idClienteChecklist: z.any(),
  
    idCliente: z.any(),
  
    idTipoArquivo: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvClientesChecklistFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvClientesChecklistForm({
  isOpen,
  onClose,
  initialValues,
}: AdvClientesChecklistFormProps) {
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

  const createMutation = useCreateAdvClientesChecklist();
const updateMutation = useUpdateAdvClientesChecklist();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advClientesChecklist updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advClientesChecklist created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advclienteschecklist:", error);
    toast.error(error.message || "Failed to save advclienteschecklist");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advClientesChecklist": "Create advClientesChecklist" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advclienteschecklist." : "Fill in the details to create a new advclienteschecklist." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idClienteChecklist" > idClienteChecklist</Label>

<Input id="idClienteChecklist" type = "number" step = "any" {...register("idClienteChecklist") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipoArquivo" > idTipoArquivo * </Label>

<Input id="idTipoArquivo" type = "number" step = "any" {...register("idTipoArquivo") } />

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
