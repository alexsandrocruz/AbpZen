import { useState } from 'react';
import { Download, X, FileCode, FolderOpen, ChevronDown, ChevronRight, Loader2, Monitor, Globe, Atom } from 'lucide-react';
import type { EntityData, FrontendTarget } from '../types';
import { codeGenerator, type GeneratedFile, type ParentRelationshipContext, type ChildRelationshipContext, type RelationshipInfo } from '../generators';
import { downloadAsZip, getFileIcon, getLayerColor } from '../generators/zip';

interface GenerateCodeModalProps {
    entities: EntityData[];
    relationships: RelationshipInfo[];
    projectName: string;
    projectNamespace: string;
    projectPath: string;
    defaultFrontends?: FrontendTarget[];
    onClose: () => void;
}

export default function GenerateCodeModal({
    entities,
    relationships,
    projectName,
    projectNamespace,
    projectPath,
    defaultFrontends,
    onClose
}: GenerateCodeModalProps) {
    const [files, setFiles] = useState<GeneratedFile[]>([]);
    const [generating, setGenerating] = useState(false);
    const [selectedFile, setSelectedFile] = useState<GeneratedFile | null>(null);
    const [expandedLayers, setExpandedLayers] = useState<Set<string>>(new Set(['Application', 'Application.Contracts']));
    const [applying, setApplying] = useState(false);
    const [injecting, setInjecting] = useState(false);
    const [applyStatus, setApplyStatus] = useState<{ success: boolean; message: string } | null>(null);
    const [entityStatus, setEntityStatus] = useState<Record<string, 'pending' | 'generating' | 'done' | 'error'>>({});
    const [generationError, setGenerationError] = useState<string | null>(null);
    const [currentEntityIndex, setCurrentEntityIndex] = useState(0);

    // Entity selection state - all selected by default
    const [selectedEntities, setSelectedEntities] = useState<Set<string>>(
        () => new Set(entities.map(e => e.name))
    );

    // Frontend selection state - use defaultFrontends if provided, else Razor
    const [selectedFrontends, setSelectedFrontends] = useState<Set<FrontendTarget>>(
        () => new Set((defaultFrontends && defaultFrontends.length > 0) ? defaultFrontends : ['razor'] as FrontendTarget[])
    );

    const camelCase = (str: string) => str.charAt(0).toLowerCase() + str.slice(1);

    // Extract short project name from namespace for ABP class naming
    // ABP uses the last segment of the namespace (e.g., 'Cursos' from 'Sapienza.Cursos')
    // The folder structure uses full namespace (Sapienza.Cursos.Web), but class names use short name (CursosMenus)
    const getShortProjectName = (name: string, namespace: string): string => {
        // If projectName already looks like a namespace (contains dots), extract last part
        if (name.includes('.')) {
            return name.split('.').pop() || name;
        }
        // If projectName is simple, use it directly
        if (name && name !== namespace) {
            return name;
        }
        // Fallback: extract from namespace
        return namespace.split('.').pop() || namespace;
    };

    const shortProjectName = getShortProjectName(projectName, projectNamespace);

    // Helper: compute relationship context for an entity
    // CONVENTION: In 1:N relationship edge:
    //   - Source = child entity (the "Many" side, has the FK)
    //   - Target = parent entity (the "One" side, has the collection)
    // Example: Product → Category means Product has CategoryId (Product is child of Category)
    const getRelationshipContext = (entityName: string) => {
        const asParent: ParentRelationshipContext[] = [];
        const asChild: ChildRelationshipContext[] = [];

        // Build entity lookup by name
        const entityMap = new Map(entities.map(e => [e.name, e]));

        for (const rel of relationships) {
            if (rel.data.type === 'one-to-many') {
                const sourceEntity = entityMap.get(rel.source); // Child (has FK)
                const targetEntity = entityMap.get(rel.target); // Parent (has collection)

                if (sourceEntity && targetEntity) {
                    // If this entity is the TARGET (parent - the "One" side)
                    if (rel.target === entityName) {
                        asParent.push({
                            childEntityName: sourceEntity.name,
                            childPluralName: sourceEntity.pluralName,
                            navigationName: rel.data.sourceNavigationName || sourceEntity.pluralName,
                            childNavigationName: rel.data.targetNavigationName || targetEntity.name,
                            childFkFieldName: `${targetEntity.name}Id`,
                        });
                    }

                    // If this entity is the SOURCE (child - the "Many" side, has FK)
                    if (rel.source === entityName) {
                        const fkField = sourceEntity.fields.find(f =>
                            f.isLookup && f.lookupConfig?.targetEntity === targetEntity.name
                        );

                        const displayFieldField = targetEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                            || targetEntity.fields.find(f => f.name === 'Title' || f.name === 'title')
                            || targetEntity.fields.find(f => f.type === 'string')
                            || { name: 'Id' };

                        asChild.push({
                            parentEntityName: targetEntity.name,
                            parentPluralName: targetEntity.pluralName,
                            fkFieldName: fkField?.name || `${targetEntity.name}Id`,
                            navigationName: rel.data.targetNavigationName || targetEntity.name,
                            parentNavigationName: rel.data.sourceNavigationName || sourceEntity.pluralName,
                            isRequired: rel.data.isRequired,
                            lookupMode: fkField?.lookupConfig?.mode || 'dropdown',
                            displayField: displayFieldField.name
                        });
                    }
                }
            } else if (rel.data.type === 'many-to-many') {
                const sourceEntity = entityMap.get(rel.source);
                const targetEntity = entityMap.get(rel.target);

                if (sourceEntity && targetEntity && rel.data.junctionConfig) {
                    const junctionName = rel.data.junctionConfig.junctionEntityId || rel.data.junctionConfig.tableName;
                    const junctionEntity = entities.find(e => e.name === junctionName);

                    if (junctionEntity) {
                        // If this entity is the source and showInSource is true
                        if (sourceEntity.name === entityName && rel.data.junctionConfig.showInSource) {
                            asParent.push({
                                childEntityName: junctionEntity.name,
                                childPluralName: junctionEntity.pluralName,
                                navigationName: rel.data.sourceNavigationName || junctionEntity.pluralName,
                                childNavigationName: sourceEntity.name,
                                childFkFieldName: `${sourceEntity.name}Id`,
                            });
                        }

                        // If this entity is the target and showInTarget is true
                        if (targetEntity.name === entityName && rel.data.junctionConfig.showInTarget) {
                            asParent.push({
                                childEntityName: junctionEntity.name,
                                childPluralName: junctionEntity.pluralName,
                                navigationName: rel.data.targetNavigationName || junctionEntity.pluralName,
                                childNavigationName: targetEntity.name,
                                childFkFieldName: `${targetEntity.name}Id`,
                            });
                        }

                        // If this entity IS the junction entity - add both source and target as parents
                        if (junctionEntity.name === entityName) {
                            // Source entity as parent
                            const sourceDisplayField = sourceEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                                || sourceEntity.fields.find(f => f.name === 'Nome')
                                || sourceEntity.fields.find(f => f.type === 'string')
                                || { name: 'Id' };

                            asChild.push({
                                parentEntityName: sourceEntity.name,
                                parentPluralName: sourceEntity.pluralName,
                                fkFieldName: rel.data.junctionConfig.sourceForeignKey || `${sourceEntity.name}Id`,
                                navigationName: sourceEntity.name,
                                parentNavigationName: junctionEntity.pluralName,
                                isRequired: true,
                                lookupMode: 'dropdown',
                                displayField: sourceDisplayField.name
                            });

                            // Target entity as parent
                            const targetDisplayField = targetEntity.fields.find(f => f.name === 'Name' || f.name === 'name')
                                || targetEntity.fields.find(f => f.name === 'Nome')
                                || targetEntity.fields.find(f => f.type === 'string')
                                || { name: 'Id' };

                            asChild.push({
                                parentEntityName: targetEntity.name,
                                parentPluralName: targetEntity.pluralName,
                                fkFieldName: rel.data.junctionConfig.targetForeignKey || `${targetEntity.name}Id`,
                                navigationName: targetEntity.name,
                                parentNavigationName: junctionEntity.pluralName,
                                isRequired: true,
                                lookupMode: 'dropdown',
                                displayField: targetDisplayField.name
                            });
                        }
                    }
                }
            }
        }

        return { asParent, asChild };
    };

    // Get all related entities (both parents and children) for a given entity
    // With convention: source=child, target=parent
    const getRelatedEntities = (entityName: string): string[] => {
        const related: Set<string> = new Set();
        for (const rel of relationships) {
            if (rel.data.type === 'one-to-many') {
                if (rel.source === entityName) {
                    related.add(rel.target); // Parent entity (we are the child)
                }
                if (rel.target === entityName) {
                    related.add(rel.source); // Child entity (we are the parent)
                }
            }
        }
        return Array.from(related);
    };

    // Toggle entity selection with relationship awareness
    const toggleEntitySelection = (entityName: string, checked: boolean) => {
        setSelectedEntities(prev => {
            const next = new Set(prev);
            if (checked) {
                next.add(entityName);
                // Auto-select related entities
                const related = getRelatedEntities(entityName);
                related.forEach(r => next.add(r));
            } else {
                next.delete(entityName);
                // Note: We don't auto-deselect related entities
                // because they might be needed by other selected entities
            }
            return next;
        });
    };

    // Select/Deselect all entities
    const toggleSelectAll = () => {
        if (selectedEntities.size === entities.length) {
            setSelectedEntities(new Set());
        } else {
            setSelectedEntities(new Set(entities.map(e => e.name)));
        }
    };

    // Toggle frontend selection
    const toggleFrontend = (frontend: FrontendTarget) => {
        setSelectedFrontends(prev => {
            const next = new Set(prev);
            if (next.has(frontend)) {
                // Don't allow deselecting all frontends
                if (next.size > 1) {
                    next.delete(frontend);
                }
            } else {
                next.add(frontend);
            }
            return next;
        });
    };

    const handleGenerate = async () => {
        setGenerating(true);
        setGenerationError(null);
        const allFiles: GeneratedFile[] = [];

        // Filter to only selected entities
        const entitiesToGenerate = entities.filter(e => selectedEntities.has(e.name));

        if (entitiesToGenerate.length === 0) {
            setGenerationError('No entities selected for generation.');
            setGenerating(false);
            return;
        }

        // Initialize selected entities as pending
        const initialStatus: Record<string, 'pending' | 'generating' | 'done' | 'error'> = {};
        entitiesToGenerate.forEach(e => { initialStatus[e.name] = 'pending'; });
        setEntityStatus(initialStatus);

        try {
            for (let i = 0; i < entitiesToGenerate.length; i++) {
                const entity = entitiesToGenerate[i];
                setCurrentEntityIndex(i);
                setEntityStatus(prev => ({ ...prev, [entity.name]: 'generating' }));

                try {
                    const { asParent, asChild } = getRelationshipContext(entity.name);
                    const entityFiles = await codeGenerator.generateEntityWithFrontends(
                        entity,
                        projectName,
                        projectNamespace,
                        asParent,
                        asChild,
                        Array.from(selectedFrontends)
                    );
                    allFiles.push(...entityFiles);
                    setEntityStatus(prev => ({ ...prev, [entity.name]: 'done' }));
                } catch (err) {
                    console.error(`Error generating ${entity.name}:`, err);
                    setEntityStatus(prev => ({ ...prev, [entity.name]: 'error' }));
                    setGenerationError(`Failed to generate ${entity.name}: ${err}`);
                }
            }

            setFiles(allFiles);
            if (allFiles.length > 0) {
                setSelectedFile(allFiles[0]);
            }
        } catch (error) {
            console.error('Generation error:', error);
            setGenerationError(`Generation failed: ${error}`);
        } finally {
            setGenerating(false);
        }
    };

    const handleGenerateSingle = async (entity: EntityData) => {
        setGenerating(true);
        setEntityStatus(prev => ({ ...prev, [entity.name]: 'generating' }));

        try {
            const { asParent, asChild } = getRelationshipContext(entity.name);
            const entityFiles = await codeGenerator.generateEntityWithFrontends(
                entity,
                projectName,
                projectNamespace,
                asParent,
                asChild,
                Array.from(selectedFrontends)
            );

            // Remove old files for this entity and add new ones
            setFiles(prev => {
                const filtered = prev.filter(f => !f.path.includes(`/${entity.name}/`) && !f.path.includes(`${entity.name}Dto`));
                return [...filtered, ...entityFiles];
            });

            setEntityStatus(prev => ({ ...prev, [entity.name]: 'done' }));
            if (entityFiles.length > 0) {
                setSelectedFile(entityFiles[0]);
            }
        } catch (err) {
            console.error(`Error generating ${entity.name}:`, err);
            setEntityStatus(prev => ({ ...prev, [entity.name]: 'error' }));
        } finally {
            setGenerating(false);
        }
    };

    const handleDownload = async () => {
        await downloadAsZip(files, projectName);
    };

    const handleApply = async () => {
        if (!projectPath) return;
        setApplying(true);
        setApplyStatus(null);
        try {
            // Separate files to overwrite and files to merge
            const normalFiles = files.filter(f => !f.path.endsWith('.json-merge'));
            const mergeFiles = files.filter(f => f.path.endsWith('.json-merge'));

            // 1. Send normal files to /api/generate-code
            if (normalFiles.length > 0) {
                const controller = new AbortController();
                const timeoutId = setTimeout(() => controller.abort(), 15000); // 15s timeout

                try {
                    const response = await fetch('http://localhost:3001/api/generate-code', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({
                            projectPath,
                            files: normalFiles.map(f => ({ path: f.path, content: f.content }))
                        }),
                        signal: controller.signal
                    });

                    clearTimeout(timeoutId);

                    if (!response.ok) {
                        const data = await response.json();
                        throw new Error(data.error || 'Failed to apply normal files');
                    }
                } catch (err: any) {
                    if (err.name === 'AbortError') throw new Error('Request timed out after 15 seconds');
                    throw err;
                }
            }

            // 2. Send merge files to /api/inject-code as 'json-merge' type
            if (mergeFiles.length > 0) {
                const instructions = mergeFiles.map(f => ({
                    file: f.path.replace('.json-merge', '.json'),
                    content: f.content,
                    type: 'json-merge' as const
                }));

                const response = await fetch('http://localhost:3001/api/inject-code', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        projectPath,
                        instructions
                    })
                });

                if (!response.ok) {
                    const data = await response.json();
                    throw new Error(data.error || 'Failed to merge localization files');
                }

                const data = await response.json();
                const failed = data.results.filter((r: any) => !r.success);
                if (failed.length > 0) {
                    throw new Error(`Merge failed: ${failed[0].error}`);
                }
            }

            // 3. Auto-inject permissions, menus, etc. (only for selected entities)
            const selectedEntityList = entities.filter(e => selectedEntities.has(e.name));
            if (selectedEntityList.length > 0) {
                const injectInstructions = selectedEntityList.flatMap(entity => [
                    {
                        file: `${projectName}.Web/Menus/${projectName}Menus.cs`,
                        marker: 'ZenCode-Menus-Marker',
                        content: `        public const string ${entity.name} = Prefix + ".${entity.name}";`
                    },
                    {
                        file: `${projectName}.Web/Menus/${projectName}MenuContributor.cs`,
                        marker: 'ZenCode-Menu-Marker',
                        content: `            context.Menu.AddItem(new ApplicationMenuItem(${projectName}Menus.${entity.name}, l["Menu:${entity.pluralName}"], "~/${entity.name}", icon: "fa fa-folder-open").RequirePermissions(${projectName}Permissions.${entity.name}.Default));`
                    },
                    {
                        file: `${projectName}.Application.Contracts/Permissions/${projectName}Permissions.cs`,
                        marker: 'ZenCode-Permissions-Marker',
                        content: `        public static class ${entity.name}\n        {\n            public const string Default = GroupName + ".${entity.name}";\n            public const string Create = Default + ".Create";\n            public const string Update = Default + ".Update";\n            public const string Delete = Default + ".Delete";\n        }`
                    },
                    {
                        file: `${projectName}.Application.Contracts/Permissions/${projectName}PermissionDefinitionProvider.cs`,
                        marker: 'ZenCode-PermissionDefinition-Marker',
                        content: `            var ${camelCase(entity.name)}Permission = myGroup.AddPermission(${projectName}Permissions.${entity.name}.Default, L("Permission:${entity.name}"));\n            ${camelCase(entity.name)}Permission.AddChild(${projectName}Permissions.${entity.name}.Create, L("Permission:Create"));\n            ${camelCase(entity.name)}Permission.AddChild(${projectName}Permissions.${entity.name}.Update, L("Permission:Update"));\n            ${camelCase(entity.name)}Permission.AddChild(${projectName}Permissions.${entity.name}.Delete, L("Permission:Delete"));`
                    }
                ]);

                const injectResponse = await fetch('http://localhost:3001/api/inject-code', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        projectPath,
                        instructions: injectInstructions
                    })
                });

                if (!injectResponse.ok) {
                    const data = await injectResponse.json();
                    throw new Error(data.error || 'Failed to inject permissions/menus');
                }

                const injectData = await injectResponse.json();
                const injectFailed = injectData.results.filter((r: any) => !r.success);
                if (injectFailed.length > 0) {
                    console.warn('Some injections failed:', injectFailed);
                }
            }

            setApplyStatus({ success: true, message: `Successfully applied ${files.length} files and injected permissions/menus!` });
        } catch (error: any) {
            console.error('Apply error:', error);
            setApplyStatus({ success: false, message: error.message || 'Failed to apply code' });
        } finally {
            setApplying(false);
        }
    };

    const handleInject = async () => {
        if (!projectPath) return;
        setInjecting(true);
        setApplyStatus(null);
        try {
            // Define instructions for each entity
            const instructions = entities.flatMap(entity => [
                {
                    file: `${projectNamespace}.Web/Menus/${shortProjectName}Menus.cs`,
                    marker: 'ZenCode-Menus-Marker',
                    content: `        public const string ${entity.name} = Prefix + ".${entity.name}";`
                },
                {
                    file: `${projectNamespace}.Web/Menus/${shortProjectName}MenuContributor.cs`,
                    marker: 'ZenCode-Menu-Marker',
                    content: `            context.Menu.AddItem(new ApplicationMenuItem(${shortProjectName}Menus.${entity.name}, l["Menu:${entity.pluralName}"], "~/${entity.name}", icon: "fa fa-folder-open").RequirePermissions(${shortProjectName}Permissions.${entity.name}.Default));`
                },
                {
                    file: `${projectNamespace}.Application.Contracts/Permissions/${shortProjectName}Permissions.cs`,
                    marker: 'ZenCode-Permissions-Marker',
                    content: `        public static class ${entity.name}\n        {\n            public const string Default = GroupName + ".${entity.name}";\n            public const string Create = Default + ".Create";\n            public const string Update = Default + ".Update";\n            public const string Delete = Default + ".Delete";\n        }`
                },
                {
                    file: `${projectNamespace}.Application.Contracts/Permissions/${shortProjectName}PermissionDefinitionProvider.cs`,
                    marker: 'ZenCode-PermissionDefinition-Marker',
                    content: `            var ${camelCase(entity.name)}Permission = myGroup.AddPermission(${shortProjectName}Permissions.${entity.name}.Default, L("Permission:${entity.name}"));\n            ${camelCase(entity.name)}Permission.AddChild(${shortProjectName}Permissions.${entity.name}.Create, L("Permission:Create"));\n            ${camelCase(entity.name)}Permission.AddChild(${shortProjectName}Permissions.${entity.name}.Update, L("Permission:Update"));\n            ${camelCase(entity.name)}Permission.AddChild(${shortProjectName}Permissions.${entity.name}.Delete, L("Permission:Delete"));`
                }
            ]);

            const response = await fetch('http://localhost:3001/api/inject-code', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    projectPath,
                    instructions
                })
            });

            if (response.ok) {
                const data = await response.json();
                const failed = data.results.filter((r: any) => !r.success);
                if (failed.length === 0) {
                    setApplyStatus({ success: true, message: 'Successfully injected all markers!' });
                } else {
                    setApplyStatus({ success: false, message: `Injected with errors: ${failed[0].error}` });
                }
            } else {
                setApplyStatus({ success: false, message: 'Failed to inject code' });
            }
        } catch (error) {
            setApplyStatus({ success: false, message: 'Bridge error' });
        } finally {
            setInjecting(false);
        }
    };

    const toggleLayer = (layer: string) => {
        setExpandedLayers(prev => {
            const next = new Set(prev);
            if (next.has(layer)) {
                next.delete(layer);
            } else {
                next.add(layer);
            }
            return next;
        });
    };

    // Group files by layer
    const filesByLayer = files.reduce((acc, file) => {
        if (!acc[file.layer]) acc[file.layer] = [];
        acc[file.layer].push(file);
        return acc;
    }, {} as Record<string, GeneratedFile[]>);

    return (
        <div className="ui-dialog-overlay" onClick={onClose}>
            <div
                className="ui-dialog"
                style={{ width: '1200px', maxHeight: '90vh' }}
                onClick={(e) => e.stopPropagation()}
            >
                <div className="ui-dialog-header">
                    <h2 className="ui-dialog-title">
                        <FileCode size={20} />
                        Generate Code
                    </h2>
                    <button className="btn-icon" onClick={onClose}>
                        <X size={20} />
                    </button>
                </div>

                <div className="ui-dialog-content" style={{ padding: 0 }}>
                    {files.length === 0 ? (
                        <div style={{ padding: '32px' }}>
                            <div style={{ textAlign: 'center', marginBottom: '24px' }}>
                                <FileCode size={48} style={{ color: '#6366f1', marginBottom: '16px' }} />
                                <h3 style={{ color: '#f8fafc', marginBottom: '8px' }}>Ready to Generate</h3>
                                <p style={{ color: '#64748b', marginBottom: '16px' }}>
                                    {entities.length} entities will be converted to ABP backend code
                                </p>
                                {generating && (
                                    <p style={{ color: '#6366f1', fontSize: '0.875rem' }}>
                                        Generating: {currentEntityIndex + 1} of {entities.length} entities...
                                    </p>
                                )}
                            </div>

                            <div style={{ marginBottom: '24px', maxWidth: '500px', margin: '0 auto 24px' }}>
                                <div className="form-group">
                                    <label>Project Name</label>
                                    <input type="text" value={projectName} disabled className="ui-input" />
                                </div>
                                <div className="form-group" style={{ marginTop: '12px' }}>
                                    <label>Namespace</label>
                                    <input type="text" value={projectNamespace} disabled className="ui-input" />
                                </div>
                            </div>

                            {/* Frontend Selection */}
                            <div style={{ maxWidth: '600px', margin: '0 auto 24px' }}>
                                <label style={{ color: '#94a3b8', fontSize: '0.875rem', marginBottom: '12px', display: 'block' }}>
                                    Frontend Targets
                                </label>
                                <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '12px' }}>
                                    {/* Razor Card */}
                                    <div
                                        onClick={() => toggleFrontend('razor')}
                                        style={{
                                            padding: '16px',
                                            background: selectedFrontends.has('razor') ? 'rgba(99, 102, 241, 0.15)' : '#0f172a',
                                            border: selectedFrontends.has('razor') ? '2px solid #6366f1' : '1px solid #334155',
                                            borderRadius: '8px',
                                            cursor: 'pointer',
                                            textAlign: 'center',
                                            transition: 'all 0.2s'
                                        }}
                                    >
                                        <Monitor size={24} style={{ color: selectedFrontends.has('razor') ? '#6366f1' : '#64748b', marginBottom: '8px' }} />
                                        <div style={{ color: '#f8fafc', fontWeight: 500, fontSize: '0.875rem' }}>Razor</div>
                                        <div style={{ color: '#64748b', fontSize: '0.75rem' }}>ASP.NET MVC</div>
                                    </div>

                                    {/* Angular Card */}
                                    <div
                                        onClick={() => toggleFrontend('angular')}
                                        style={{
                                            padding: '16px',
                                            background: selectedFrontends.has('angular') ? 'rgba(220, 38, 38, 0.15)' : '#0f172a',
                                            border: selectedFrontends.has('angular') ? '2px solid #dc2626' : '1px solid #334155',
                                            borderRadius: '8px',
                                            cursor: 'pointer',
                                            textAlign: 'center',
                                            transition: 'all 0.2s'
                                        }}
                                    >
                                        <Globe size={24} style={{ color: selectedFrontends.has('angular') ? '#dc2626' : '#64748b', marginBottom: '8px' }} />
                                        <div style={{ color: '#f8fafc', fontWeight: 500, fontSize: '0.875rem' }}>Angular</div>
                                        <div style={{ color: '#64748b', fontSize: '0.75rem' }}>Angular 17+</div>
                                    </div>

                                    {/* React Card */}
                                    <div
                                        onClick={() => toggleFrontend('react')}
                                        style={{
                                            padding: '16px',
                                            background: selectedFrontends.has('react') ? 'rgba(6, 182, 212, 0.15)' : '#0f172a',
                                            border: selectedFrontends.has('react') ? '2px solid #06b6d4' : '1px solid #334155',
                                            borderRadius: '8px',
                                            cursor: 'pointer',
                                            textAlign: 'center',
                                            transition: 'all 0.2s'
                                        }}
                                    >
                                        <Atom size={24} style={{ color: selectedFrontends.has('react') ? '#06b6d4' : '#64748b', marginBottom: '8px' }} />
                                        <div style={{ color: '#f8fafc', fontWeight: 500, fontSize: '0.875rem' }}>React</div>
                                        <div style={{ color: '#64748b', fontSize: '0.75rem' }}>Next.js 15</div>
                                    </div>
                                </div>
                                <div style={{ color: '#64748b', fontSize: '0.75rem', marginTop: '8px', textAlign: 'center' }}>
                                    {selectedFrontends.size} frontend(s) selected • Backend always included
                                </div>
                            </div>

                            {/* Entity List with Status */}
                            <div style={{
                                maxWidth: '500px',
                                margin: '0 auto 24px',
                                background: '#0f172a',
                                borderRadius: '8px',
                                border: '1px solid #334155',
                                overflow: 'hidden'
                            }}>
                                <div style={{
                                    padding: '12px 16px',
                                    background: '#1e293b',
                                    borderBottom: '1px solid #334155',
                                    display: 'flex',
                                    justifyContent: 'space-between',
                                    alignItems: 'center'
                                }}>
                                    <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                                        <input
                                            type="checkbox"
                                            checked={selectedEntities.size === entities.length}
                                            onChange={toggleSelectAll}
                                            style={{ cursor: 'pointer' }}
                                        />
                                        <span style={{ color: '#94a3b8', fontSize: '0.875rem' }}>
                                            Entities ({selectedEntities.size} / {entities.length} selected)
                                        </span>
                                    </div>
                                    <span style={{ color: '#64748b', fontSize: '0.75rem' }}>
                                        {Object.values(entityStatus).filter(s => s === 'done').length} done
                                    </span>
                                </div>
                                <div style={{ maxHeight: '200px', overflow: 'auto' }}>
                                    {entities.map((entity, idx) => {
                                        const status = entityStatus[entity.name] || 'pending';
                                        const isSelected = selectedEntities.has(entity.name);
                                        const relatedEntities = getRelatedEntities(entity.name);
                                        const hasRelationships = relatedEntities.length > 0;
                                        return (
                                            <div
                                                key={entity.name}
                                                style={{
                                                    padding: '10px 16px',
                                                    display: 'flex',
                                                    alignItems: 'center',
                                                    justifyContent: 'space-between',
                                                    borderBottom: idx < entities.length - 1 ? '1px solid #1e293b' : 'none',
                                                    background: status === 'generating' ? 'rgba(99, 102, 241, 0.1)' :
                                                        !isSelected ? 'rgba(100, 116, 139, 0.1)' : 'transparent',
                                                    opacity: isSelected ? 1 : 0.6
                                                }}
                                            >
                                                <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                                                    <input
                                                        type="checkbox"
                                                        checked={isSelected}
                                                        onChange={(e) => toggleEntitySelection(entity.name, e.target.checked)}
                                                        style={{ cursor: 'pointer' }}
                                                    />
                                                    {status === 'pending' && isSelected && <span style={{ color: '#64748b' }}>⏳</span>}
                                                    {status === 'generating' && <Loader2 size={14} className="animate-spin" style={{ color: '#6366f1' }} />}
                                                    {status === 'done' && <span style={{ color: '#22c55e' }}>✅</span>}
                                                    {status === 'error' && <span style={{ color: '#ef4444' }}>❌</span>}
                                                    <span style={{ color: '#f8fafc', fontSize: '0.875rem' }}>{entity.name}</span>
                                                    <span style={{ color: '#64748b', fontSize: '0.75rem' }}>
                                                        ({entity.fields.length} fields)
                                                    </span>
                                                    {hasRelationships && (
                                                        <span
                                                            title={`Related: ${relatedEntities.join(', ')}`}
                                                            style={{
                                                                color: '#6366f1',
                                                                fontSize: '0.7rem',
                                                                background: 'rgba(99, 102, 241, 0.15)',
                                                                padding: '2px 6px',
                                                                borderRadius: '4px'
                                                            }}
                                                        >
                                                            🔗 {relatedEntities.length}
                                                        </span>
                                                    )}
                                                </div>
                                                <button
                                                    className="btn-icon-sm"
                                                    onClick={() => handleGenerateSingle(entity)}
                                                    disabled={generating}
                                                    title={`Generate ${entity.name}`}
                                                    style={{ opacity: generating ? 0.5 : 1 }}
                                                >
                                                    <FileCode size={14} />
                                                </button>
                                            </div>
                                        );
                                    })}
                                </div>
                            </div>

                            {generationError && (
                                <div style={{
                                    maxWidth: '500px',
                                    margin: '0 auto 16px',
                                    padding: '12px',
                                    background: 'rgba(239, 68, 68, 0.1)',
                                    borderRadius: '6px',
                                    color: '#f87171',
                                    fontSize: '0.875rem'
                                }}>
                                    {generationError}
                                </div>
                            )}

                            <div style={{ textAlign: 'center' }}>
                                <button
                                    className="ui-button ui-button-primary"
                                    onClick={handleGenerate}
                                    disabled={generating || selectedEntities.size === 0}
                                    style={{ padding: '12px 32px' }}
                                >
                                    {generating ? (
                                        <>
                                            <Loader2 size={18} className="animate-spin" />
                                            Generating {currentEntityIndex + 1}/{selectedEntities.size}...
                                        </>
                                    ) : (
                                        <>
                                            <FileCode size={18} />
                                            Generate Selected ({selectedEntities.size})
                                        </>
                                    )}
                                </button>
                            </div>
                        </div>
                    ) : (
                        <div style={{ display: 'flex', height: '500px' }}>
                            {/* File tree */}
                            <div style={{
                                width: '300px',
                                borderRight: '1px solid #334155',
                                overflow: 'auto',
                                background: '#0f172a'
                            }}>
                                <div style={{ padding: '12px', borderBottom: '1px solid #334155' }}>
                                    <span style={{ color: '#64748b', fontSize: '0.75rem' }}>
                                        {files.length} files generated
                                    </span>
                                </div>

                                {Object.entries(filesByLayer).map(([layer, layerFiles]) => (
                                    <div key={layer}>
                                        <div
                                            style={{
                                                padding: '8px 12px',
                                                display: 'flex',
                                                alignItems: 'center',
                                                gap: '8px',
                                                cursor: 'pointer',
                                                background: '#1e293b',
                                                borderBottom: '1px solid #334155',
                                            }}
                                            onClick={() => toggleLayer(layer)}
                                        >
                                            {expandedLayers.has(layer) ? <ChevronDown size={14} /> : <ChevronRight size={14} />}
                                            <FolderOpen size={14} style={{ color: getLayerColor(layer as any) }} />
                                            <span style={{ color: '#f8fafc', fontSize: '0.875rem' }}>{layer}</span>
                                            <span style={{ color: '#64748b', fontSize: '0.75rem', marginLeft: 'auto' }}>
                                                {layerFiles.length}
                                            </span>
                                        </div>

                                        {expandedLayers.has(layer) && (
                                            <div>
                                                {layerFiles.map(file => {
                                                    const fileName = file.path.split('/').pop();
                                                    return (
                                                        <div
                                                            key={file.path}
                                                            style={{
                                                                padding: '6px 12px 6px 32px',
                                                                display: 'flex',
                                                                alignItems: 'center',
                                                                gap: '8px',
                                                                cursor: 'pointer',
                                                                background: selectedFile?.path === file.path ? '#334155' : 'transparent',
                                                            }}
                                                            onClick={() => setSelectedFile(file)}
                                                        >
                                                            <span>{getFileIcon(file.path)}</span>
                                                            <span style={{
                                                                color: selectedFile?.path === file.path ? '#f8fafc' : '#94a3b8',
                                                                fontSize: '0.8125rem',
                                                                overflow: 'hidden',
                                                                textOverflow: 'ellipsis',
                                                                whiteSpace: 'nowrap',
                                                            }}>
                                                                {fileName}
                                                            </span>
                                                        </div>
                                                    );
                                                })}
                                            </div>
                                        )}
                                    </div>
                                ))}
                            </div>

                            {/* Code preview */}
                            <div style={{ flex: 1, overflow: 'auto', background: '#0f172a' }}>
                                {selectedFile && (
                                    <>
                                        <div style={{
                                            padding: '8px 16px',
                                            borderBottom: '1px solid #334155',
                                            background: '#1e293b',
                                        }}>
                                            <span style={{ color: '#64748b', fontSize: '0.75rem' }}>
                                                {selectedFile.path}
                                            </span>
                                        </div>
                                        <pre style={{
                                            margin: 0,
                                            padding: '16px',
                                            color: '#e2e8f0',
                                            fontSize: '0.8125rem',
                                            lineHeight: '1.6',
                                            fontFamily: 'JetBrains Mono, Consolas, monospace',
                                        }}>
                                            {selectedFile.content}
                                        </pre>
                                    </>
                                )}
                            </div>
                        </div>
                    )}
                </div>

                {files.length > 0 && (
                    <div className="ui-dialog-footer">
                        <button className="ui-button ui-button-secondary" onClick={() => setFiles([])}>
                            Regenerate
                        </button>
                        {projectPath && (
                            <div style={{ display: 'flex', gap: '8px' }}>
                                <button
                                    className="ui-button ui-button-primary"
                                    onClick={handleApply}
                                    disabled={applying}
                                    style={{ background: 'linear-gradient(135deg, #22c55e 0%, #16a34a 100%)' }}
                                >
                                    {applying ? <Loader2 size={18} className="animate-spin" /> : <FolderOpen size={18} />}
                                    Write Files
                                </button>
                                <button
                                    className="ui-button ui-button-primary"
                                    onClick={handleInject}
                                    disabled={injecting}
                                    style={{ background: 'linear-gradient(135deg, #6366f1 0%, #a855f7 100%)' }}
                                >
                                    {injecting ? <Loader2 size={18} className="animate-spin" /> : <FileCode size={18} />}
                                    Smart Inject
                                </button>
                            </div>
                        )}
                        <button className="ui-button ui-button-secondary" onClick={handleDownload} style={{ background: 'transparent', border: '1px solid #334155' }}>
                            <Download size={18} />
                            Download ZIP
                        </button>
                    </div>
                )}
                {applyStatus && (
                    <div style={{
                        padding: '10px 24px',
                        background: applyStatus.success ? 'rgba(34, 197, 94, 0.1)' : 'rgba(239, 68, 68, 0.1)',
                        color: applyStatus.success ? '#4ade80' : '#f87171',
                        fontSize: '0.8rem',
                        borderTop: '1px solid #334155',
                        display: 'flex',
                        alignItems: 'center',
                        gap: '8px'
                    }}>
                        {applyStatus.success ? <FolderOpen size={14} /> : <X size={14} />}
                        {applyStatus.message}
                    </div>
                )}
            </div>
        </div >
    );
}
