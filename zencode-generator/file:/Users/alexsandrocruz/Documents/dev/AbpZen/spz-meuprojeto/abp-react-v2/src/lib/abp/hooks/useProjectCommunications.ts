import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetProjectCommunicationsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  projectId?: string;
  }

export function useProjectCommunications(input: GetProjectCommunicationsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["projectCommunications", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/project-communication", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllProjectCommunications() {
  return useQuery({
    queryKey: ["projectCommunications", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/project-communication", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useProjectCommunication(id: string) {
  return useQuery({
    queryKey: ["projectCommunication", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/project-communication/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateProjectCommunication() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/project-communication", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["projectCommunications"] });
    },
  });
}

export function useUpdateProjectCommunication() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/project-communication/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["projectCommunications"] });
      queryClient.invalidateQueries({ queryKey: ["projectCommunication", data.id] });
    },
  });
}

export function useDeleteProjectCommunication() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/project-communication/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["projectCommunications"] });
    },
  });
}


