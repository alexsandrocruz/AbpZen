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
  import { useCreateLogCampos, useUpdateLogCampos } from "@/lib/abp/hooks/useLogCamposes";
import { toast } from "sonner";

const formSchema = z.object({
  
    idLogCampo: z.any(),
  
    idLog: z.any(),
  
    campo: z.any(),
  
    dadoAnterior: z.any(),
  
    dadoNovo: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface LogCamposFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function LogCamposForm({
  isOpen,
  onClose,
  initialValues,
}: LogCamposFormProps) {
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

  const createMutation = useCreateLogCampos();
const updateMutation = useUpdateLogCampos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("logCampos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("logCampos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save logcampos:", error);
    toast.error(error.message || "Failed to save logcampos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit logCampos": "Create logCampos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the logcampos." : "Fill in the details to create a new logcampos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idLogCampo" > idLogCampo</Label>

<Input id="idLogCampo" type = "number" step = "any" {...register("idLogCampo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idLog" > idLog * </Label>

<Input id="idLog" type = "number" step = "any" {...register("idLog") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="campo" > campo</Label>

<Input id="campo" {...register("campo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dadoAnterior" > dadoAnterior</Label>

<Input id="dadoAnterior" {...register("dadoAnterior") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dadoNovo" > dadoNovo</Label>

<Input id="dadoNovo" {...register("dadoNovo") } />

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
