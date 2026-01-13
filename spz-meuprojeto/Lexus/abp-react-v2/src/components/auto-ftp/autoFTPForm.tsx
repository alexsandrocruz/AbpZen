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
  import { useCreateAutoFTP, useUpdateAutoFTP } from "@/lib/abp/hooks/useAutoFTPs";
import { toast } from "sonner";

const formSchema = z.object({
  
    id: z.any(),
  
    arquivo: z.any(),
  
    processado: z.any(),
  
    tsInclusao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AutoFTPFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AutoFTPForm({
  isOpen,
  onClose,
  initialValues,
}: AutoFTPFormProps) {
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

  const createMutation = useCreateAutoFTP();
const updateMutation = useUpdateAutoFTP();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("autoFTP updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("autoFTP created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save autoftp:", error);
    toast.error(error.message || "Failed to save autoftp");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit autoFTP": "Create autoFTP" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the autoftp." : "Fill in the details to create a new autoftp." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="id" > id</Label>

<Input id="id" type = "number" step = "any" {...register("id") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="arquivo" > arquivo</Label>

<Input id="arquivo" {...register("arquivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="processado" > processado</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="processado"
checked = { watch("processado") }
onCheckedChange = {(checked) => setValue("processado", !!checked)}
              />
  < label htmlFor = "processado" className = "text-sm font-normal" >
    { watch("processado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

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
