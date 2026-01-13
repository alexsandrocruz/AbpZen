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
  import { useCreateUsuAcessos, useUpdateUsuAcessos } from "@/lib/abp/hooks/useUsuAcessoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idAcesso: z.any(),
  
    idUsuario: z.any(),
  
    data: z.any(),
  
    ip: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface UsuAcessosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function UsuAcessosForm({
  isOpen,
  onClose,
  initialValues,
}: UsuAcessosFormProps) {
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

  const createMutation = useCreateUsuAcessos();
const updateMutation = useUpdateUsuAcessos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("usuAcessos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("usuAcessos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save usuacessos:", error);
    toast.error(error.message || "Failed to save usuacessos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit usuAcessos": "Create usuAcessos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the usuacessos." : "Fill in the details to create a new usuacessos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idAcesso" > idAcesso</Label>

<Input id="idAcesso" type = "number" step = "any" {...register("idAcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario * </Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="data" > data</Label>

<Input id="data" type = "date" {...register("data") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ip" > ip</Label>

<Input id="ip" {...register("ip") } />

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
