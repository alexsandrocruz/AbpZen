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
  import { useCreateUsuPermissoes, useUpdateUsuPermissoes } from "@/lib/abp/hooks/useUsuPermissoeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idUsuarioPermissao: z.any(),
  
    idUsuario: z.any(),
  
    idPermissao: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface UsuPermissoesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function UsuPermissoesForm({
  isOpen,
  onClose,
  initialValues,
}: UsuPermissoesFormProps) {
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

  const createMutation = useCreateUsuPermissoes();
const updateMutation = useUpdateUsuPermissoes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("usuPermissoes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("usuPermissoes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save usupermissoes:", error);
    toast.error(error.message || "Failed to save usupermissoes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit usuPermissoes": "Create usuPermissoes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the usupermissoes." : "Fill in the details to create a new usupermissoes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idUsuarioPermissao" > idUsuarioPermissao</Label>

<Input id="idUsuarioPermissao" type = "number" step = "any" {...register("idUsuarioPermissao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario</Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPermissao" > idPermissao</Label>

<Input id="idPermissao" type = "number" step = "any" {...register("idPermissao") } />

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
